using SimpleSharpLexer;

using VirtualMachine;

namespace Parser;

public class Parser
{
    private readonly Queue<Token> _tokens;
    private readonly InstructionsBuilder _builder;

    public Parser(IEnumerable<Token> tokens)
    {
        _tokens = new Queue<Token>(tokens);
        _builder = new InstructionsBuilder();
    }

    /// <summary>
    /// Разбирает программу и возвращает список инструкций для виртуальной машины
    /// </summary>
    /// <returns>Список инструкций для виртуальной машины</returns>
    /// <exception cref="SyntaxException">Ошибка синтаксического анализа</exception>
    public List<Instruction> Compile()
    {
        ParseProgram();
        return _builder.Build();
    }

    /// <summary>
    /// program = "void", "main", "(", ")", block ;
    /// </summary>
    private void ParseProgram()
    {
        Match(TokenType.Void);
        Match(TokenType.Identifier, "main");
        Match(TokenType.OpenParenthesis);
        Match(TokenType.CloseParenthesis);
        ParseBlock();
    }

    /// <summary>
    /// block = "{", { statement },  "}" ;
    /// </summary>
    private void ParseBlock()
    {
        Match(TokenType.OpenBrace);

        while (_tokens.TryPeek(out Token token) && token.Type != TokenType.CloseBrace)
        {
            ParseStatement();
        }

        Match(TokenType.CloseBrace);
    }

    /// <summary>
    /// statement = block | empty_statement | function_call ;
    /// </summary>
    private void ParseStatement()
    {
        if (!_tokens.TryPeek(out Token token))
        {
            throw new SyntaxException("Ожидалась лексема, а встречен конец файла");
        }

        switch (token.Type)
        {
            case TokenType.OpenBrace:
                ParseBlock();
                break;
            case TokenType.Semicolon:
                _tokens.Dequeue();
                break;
            default:
                ParseFunctionCall();
                break;
        }
    }

    /// <summary>
    /// function_call = write_call ;
    /// </summary>
    private void ParseFunctionCall()
    {
        ParseWriteCall();
    }

    /// <summary>
    /// write_call = "Write", "(", expression, ")", ";" ;
    /// </summary>
    private void ParseWriteCall()
    {
        Match(TokenType.Identifier, "Write");
        Match(TokenType.OpenParenthesis);
        ParseExpression();
        Match(TokenType.CloseParenthesis);
        Match(TokenType.Semicolon);
        _builder.Write();
    }

    /// <summary>
    /// expression = additive_expression ;
    /// </summary>
    private void ParseExpression()
    {
        ParseAdditiveExpression();
    }

    /// <summary>
    /// additive_expression = multiplicative_expression, { ( "+" | "-" ), multiplicative_expression } ;
    /// </summary>
    private void ParseAdditiveExpression()
    {
        ParseMultiplicativeExpression();
        while (_tokens.TryPeek(out Token token))
        {
            switch (token.Type)
            {
                case TokenType.Plus:
                    _tokens.Dequeue();
                    ParseMultiplicativeExpression();
                    _builder.Add();
                    break;
                case TokenType.Minus:
                    _tokens.Dequeue();
                    ParseMultiplicativeExpression();
                    _builder.Subtract();
                    break;
                default:
                    return;
            }
        }
    }

    /// <summary>
    /// multiplicative_expression = exponentiation_expression, { ( "*" | "/" | "%" ), exponentiation_expression } ;
    /// </summary>
    private void ParseMultiplicativeExpression()
    {
        ParseExponentiationExpression();
        while (_tokens.TryPeek(out Token token))
        {
            switch (token.Type)
            {
                case TokenType.Multiply:
                    _tokens.Dequeue();
                    ParseExponentiationExpression();
                    _builder.Multiply();
                    break;
                case TokenType.Divide:
                    _tokens.Dequeue();
                    ParseExponentiationExpression();
                    _builder.Divide();
                    break;
                case TokenType.Percent:
                    _tokens.Dequeue();
                    ParseExponentiationExpression();
                    _builder.DivWithRemainder();
                    break;
                default:
                    return;
            }
        }
    }

    /// <summary>
    /// exponentiation_expression = unary_expression, { "^", unary_expression } ;
    /// </summary>
    private void ParseExponentiationExpression()
    {
        ParseUnaryExpression();
        while (_tokens.TryPeek(out Token token))
        {
            switch (token.Type)
            {
                case TokenType.Caret:
                    _tokens.Dequeue();
                    ParseUnaryExpression();
                    _builder.Exponent();
                    break;
                default:
                    return;
            }
        }
    }

    /// <summary>
    /// unary_expression = "-", unary_expression | primary_expression ;
    /// </summary>
    private void ParseUnaryExpression()
    {
        if (!_tokens.TryPeek(out Token token))
        {
            throw new SyntaxException("Ожидалась лексема, встречен конец файла");
        }

        switch (token.Type)
        {
            case TokenType.Minus:
                _tokens.Dequeue();
                ParseUnaryExpression();
                _builder.Negate();
                break;
            default:
                ParsePrimaryExpression();
                break;
        }
    }

    /// <summary>
    /// primary_expression = int_literal | "(", expression, ")" ;
    /// </summary>
    private void ParsePrimaryExpression()
    {
        if (!_tokens.TryPeek(out Token token))
        {
            throw new SyntaxException("Ожидалась лексема, встречен конец файла");
        }

        switch (token.Type)
        {
            case TokenType.IntLiteral:
                _builder.Push(GetIntFromToken(_tokens.Dequeue()));
                break;
            case TokenType.OpenParenthesis:
                _tokens.Dequeue();
                ParseExpression();
                Match(TokenType.CloseParenthesis);
                break;
            default:
                throw new SyntaxException(
                    $"Ожидался {TokenType.IntLiteral} или {TokenType.OpenParenthesis}, встречен {token.Type}");
        }
    }

    private void Match(TokenType type)
    {
        if (!_tokens.TryDequeue(out Token token))
        {
            throw new SyntaxException(
                $"Ожидалась лексема {type}");
        }

        if (token.Type != type)
        {
            throw new SyntaxException($"Ожидалась лексема {type}, встречена {token.Type}");
        }
    }

    private void Match(TokenType type, string value)
    {
        if (!_tokens.TryDequeue(out Token token))
        {
            throw new SyntaxException(
                $"Ожидалась лексема {type} со значением {value}");
        }

        if (token.Type != type || token.Value != value)
        {
            throw new SyntaxException(
                $"Ожидалась лексема {type} со значением {value}, встречена {token.Type} со значением {token.Value}");
        }
    }

    private int GetIntFromToken(Token token)
    {
        if (!int.TryParse(token.Value, out int result))
        {
            throw new SyntaxException($"Целочисленный литерал со значением {token.Value} слишком большой");
        }

        return result;
    }
}
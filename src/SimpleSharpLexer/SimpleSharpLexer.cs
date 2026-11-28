using Lexer;
using Lexer.Abstractions;

using TextReader = Lexer.TextReader;

namespace SimpleSharpLexer;

/// <summary>
/// Лексер языка SimpleSharp
/// </summary>
public class SimpleSharpLexer
{
    private readonly AbstractLexer<LexerState, Token, TokenType> _lexer;

    public SimpleSharpLexer()
    {
        _lexer = new Lexer<LexerState, Token, TokenType>()
        {
            StateTransitionRules = SimpleSharpRules.TransitionRules,
            ErrorFunc = SimpleSharpRules.ErrorFunc,
            TokenGenerationFunc = (type, value, context) => new Token(type, value)
            {
                Line = context.Line, Column = context.Column,
            },
        };
    }

    public IReadOnlyList<Token> ParseSourceCode(string sourceCode)
    {
        TextReader reader = new(sourceCode);
        return _lexer.GetTokens(reader);
    }
}
using System.Text;

using Lexer;
using Lexer.Abstractions;

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

    public IReadOnlyList<Token> ParseSourceCode(Stream fileStream)
    {
        const int bufferSize = 4096;

        ArgumentNullException.ThrowIfNull(fileStream);

        if (!fileStream.CanRead)
        {
            throw new ArgumentException("Поток недоступен для чтения", nameof(fileStream));
        }

        if (fileStream.CanSeek)
        {
            fileStream.Seek(0, SeekOrigin.Begin);
        }

        using StreamPerSymbolReader fileReader = new(fileStream, Encoding.UTF8, bufferSize);

        return _lexer.GetTokens(fileReader);
    }
}
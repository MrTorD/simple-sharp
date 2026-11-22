using Lexer.Abstractions;

namespace Lexer.Commands;

public record PopTokenCommand<TTokenType>(TTokenType TokenType) : ILexerCommand<TTokenType>
    where TTokenType : struct, Enum
{
    public void Execute(ILexerCommandContext<TTokenType> context)
    {
        context.AddToken(TokenType);
        context.TokenBuffer.Clear();
    }
}
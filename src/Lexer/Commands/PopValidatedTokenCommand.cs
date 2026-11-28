using Lexer.Abstractions;

namespace Lexer.Commands;

public class PopValidatedTokenCommand<TTokenType> : ILexerCommand<TTokenType>
    where TTokenType : struct, Enum
{
    private Func<string, bool> _validation;
    private TTokenType _successToken;
    private TTokenType _failureToken;

    public PopValidatedTokenCommand(
        Func<string, bool> validation,
        TTokenType successToken,
        TTokenType failureToken)
    {
        _validation = validation;
        _successToken = successToken;
        _failureToken = failureToken;
    }

    public void Execute(ILexerCommandContext<TTokenType> context)
    {
        TTokenType token = _validation.Invoke(context.TokenBuffer.ToString())
            ? _successToken
            : _failureToken;

        context.AddToken(token);
        context.TokenBuffer.Clear();
    }
}
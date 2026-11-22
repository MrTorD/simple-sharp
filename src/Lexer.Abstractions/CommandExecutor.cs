namespace Lexer.Abstractions;

internal sealed class CommandExecutor<TTokenType>
    where TTokenType : struct, Enum
{
    private readonly Action<ILexerCommandContext<TTokenType>> _action;

    internal CommandExecutor(Action<ILexerCommandContext<TTokenType>> action)
    {
        _action = action;
    }

    internal void Execute(ILexerCommandContext<TTokenType> context)
    {
        _action.Invoke(context);
    }
}
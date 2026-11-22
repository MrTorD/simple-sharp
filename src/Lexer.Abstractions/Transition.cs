using System.Diagnostics.CodeAnalysis;

namespace Lexer.Abstractions;

public class Transition<TState, TTokenType>
    where TState : struct, Enum
    where TTokenType : struct, Enum
{
    [SetsRequiredMembers]
    public Transition(TState state, Action<LexerCommandBuilder<TTokenType>> commandBuilderAction)
    {
        Commands = new LexerCommandBuilder<TTokenType>();
        commandBuilderAction.Invoke(Commands);
        State = state;
    }

    public required TState State { get; init; }

    public required LexerCommandBuilder<TTokenType> Commands { get; init; }
}
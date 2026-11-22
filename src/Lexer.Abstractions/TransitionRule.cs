namespace Lexer.Abstractions;

public class TransitionRule<TState, TTokenType>
    where TState : struct, Enum
    where TTokenType : struct, Enum
{
    public required Func<char, bool> Condition { get; init; }

    public required Transition<TState, TTokenType> Transition { get; init; }
}
using Lexer.Abstractions;

namespace Lexer;

/// <summary>
/// Реализация лексера - конечного автомата.
/// Посимвольно читает текст, переходит в новое состояние,
/// основанное на текущем состоянии и полученном символе.
/// Выполняет команды из списка команд после каждого перехода
/// </summary>
/// <typeparam name="TState">Состояние лексера</typeparam>
/// <typeparam name="TToken">Токен</typeparam>
/// <typeparam name="TTokenType">Тип токена</typeparam>
public class Lexer<TState, TToken, TTokenType> : AbstractLexer<TState, TToken, TTokenType>
    where TState : struct, Enum
    where TTokenType : struct, Enum
{
    public Lexer()
        : this(new StringBuilderTokenBuffer())
    {
    }

    /// <inheritdoc/>
    public Lexer(ITokenBuffer tokenBuffer)
        : base(tokenBuffer)
    {
    }

    public override IReadOnlyList<TToken> GetTokens(IFileReader fileReader)
    {
        List<TToken> tokens = [];
        LexerContext context = new(tokens, TokenGenerationFunc) { Line = 1, Column = 1, TokenBuffer = TokenBuffer };

        while (fileReader.TryNext(out char? c))
        {
            context.CurrentChar = c.Value;

            if (!StateTransitionRules.TryGetValue(
                    context.State,
                    out IReadOnlyCollection<TransitionRule<TState, TTokenType>>? stateTransitionRules))
            {
                return InvokeErrorFunc(tokens, context, c.Value);
            }

            TransitionRule<TState, TTokenType>? transitionRule =
                stateTransitionRules.FirstOrDefault(rule => rule.Condition.Invoke(c.Value));

            if (transitionRule is null)
            {
                return InvokeErrorFunc(tokens, context, c.Value);
            }

            Transition<TState, TTokenType> transition = transitionRule.Transition;
            context.State = transition.State;

            if (!transition.Commands.IsEmpty)
            {
                CommandExecutor<TTokenType> commandsExecutor = transition.Commands.GetExecutor();
                commandsExecutor.Execute(context);
            }
        }

        return tokens;
    }

    private IReadOnlyList<TToken> InvokeErrorFunc(List<TToken> tokens, LexerContext context, char c)
    {
        (TTokenType tokenType, string message) = ErrorFunc.Invoke(context.State, c);

        tokens.Add(TokenGenerationFunc.Invoke(tokenType, message, context));
        return tokens;
    }

    private record LexerContext(
        List<TToken> Tokens,
        Func<TTokenType, string?, ILexerContext, TToken> TokenGenerationFunc)
        : ILexerCommandContext<TTokenType>, ILexerContext
    {
        public required ITokenBuffer TokenBuffer { get; init; }

        public required int Line { get; set; }

        public required int Column { get; set; }

        public char CurrentChar { get; set; }

        public TState State { get; set; }

        public void AddToken(TTokenType tokenType)
        {
            TToken token = TokenGenerationFunc.Invoke(
                tokenType,
                TokenBuffer.Length != 0
                    ? TokenBuffer.ToString()
                    : null,
                this);
            Tokens.Add(token);
        }
    }
}
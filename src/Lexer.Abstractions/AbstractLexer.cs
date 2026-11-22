namespace Lexer.Abstractions;

/// <summary>
/// Лексер-каркас
/// </summary>
/// <typeparam name="TState">Состояние лексера</typeparam>
/// <typeparam name="TToken">Токен</typeparam>
/// <typeparam name="TTokenType">Тип токена</typeparam>
public abstract class AbstractLexer<TState, TToken, TTokenType>
    where TState : struct, Enum
    where TTokenType : struct, Enum
{
    /// <summary>
    /// Создает новый экземпляр класса <see cref="AbstractLexer{TState,TToken,TTokenType}"/>.
    /// </summary>
    /// <param name="tokenBuffer">
    /// Буфер для временного хранения символов при создании токенов.
    /// Должен быть пустым и не использоваться где-либо ещё, вне одного экземпляра класса <see cref="AbstractLexer{TState,TToken,TTokenType}"/>
    /// </param>
    /// <exception cref="ArgumentNullException">Выбрасывается в случае передачи <c>null</c> в качестве <see cref="tokenBuffer"/></exception>
    protected AbstractLexer(ITokenBuffer tokenBuffer)
    {
        ArgumentNullException.ThrowIfNull(tokenBuffer);
        TokenBuffer = tokenBuffer;
    }

    /// <summary>
    /// Список переходов из состояния в состояние
    /// </summary>
    public required IReadOnlyDictionary<TState, IReadOnlyCollection<TransitionRule<TState, TTokenType>>>
        StateTransitionRules { get; init; }

    /// <summary>
    /// Функция генерации токена на основе текущего состояния и контекста лексера
    /// </summary>
    public required Func<TTokenType, string?, ILexerContext, TToken> TokenGenerationFunc { get; init; }

    /// <summary>
    /// Функция ошибки, вызываемая в случае, если не найден походящий переход
    /// </summary>
    public required Func<TState, char, (TTokenType Type, string Message)> ErrorFunc { get; init; }

    /// <summary>
    /// Буфер символов, используемый для генерации токенов
    /// </summary>
    protected ITokenBuffer TokenBuffer { get; }

    /// <summary>
    /// Функция токенизации
    /// </summary>
    /// <param name="fileReader">Поток, из которого ведется чтение</param>
    /// <returns>Список токенов</returns>
    public abstract IReadOnlyList<TToken> GetTokens(IFileReader fileReader);
}
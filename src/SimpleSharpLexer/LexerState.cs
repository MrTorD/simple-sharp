namespace SimpleSharpLexer;

/// <summary>
/// Состояния лексера языка SimpleSharp
/// </summary>
public enum LexerState
{
    /// <summary>
    /// Основное состояние
    /// </summary>
    Main,

    /// <summary>
    /// Состояние чтения идентификатора или ключевого слова
    /// </summary>
    IdentifierOrKeyword,

    /// <summary>
    /// Состояние чтения числового литерала
    /// </summary>
    IntLiteral,

    /// <summary>
    /// Состояние чтения символьного литерала
    /// </summary>
    CharLiteral,

    /// <summary>
    /// Состояние чтения символа Escape-последовательности в символьном литерале
    /// </summary>
    CharEscapeCharacter,

    /// <summary>
    /// Состояние полученного символа в символьном литерале. Ожидается закрывающая кавычка
    /// </summary>
    CharReceived,

    /// <summary>
    /// Состояние чтения строкового литерала
    /// </summary>
    StringLiteral,

    /// <summary>
    /// Состояние чтения символа Escape-последовательности в строковом литерале
    /// </summary>
    StringEscapeCharacter,

    /// <summary>
    /// Состояние полученнго символа '\r'. Если следующий символ '\n',
    /// то переход не вызовет изменение счётчика строк
    /// </summary>
    CarriageReturnReceived,

    /// <summary>
    /// Состояние полученнго символа `/`. Возможен переход к комментарию или к оператору деления
    /// </summary>
    SlashReceived,

    /// <summary>
    /// Состояние чтения комментария
    /// </summary>
    Comment,

    /// <summary>
    /// Состояние полученного символа '|'. Ожидается еще один символ '|'
    /// </summary>
    Or,

    /// <summary>
    /// Состояние полученного символа '&'. Ожидается еще один символ '&'
    /// </summary>
    And,

    /// <summary>
    /// Состояние полученного разделителя.
    /// Разделители: `(`, `)`,`{`, `}`,`[`, `]`,
    /// `+`, `-`, `*`, `/`,
    /// `.`, `,`, `;`
    /// </summary>
    Divider,

    /// <summary>
    /// Состояние полученного `!`. Ожидается `=`
    /// </summary>
    Exclamation,

    /// <summary>
    /// Состояние полученного `=`. Ожидается `=`
    /// </summary>
    Equals,

    /// <summary>
    /// Состояние полученного `&lt;`. Ожидается `=`
    /// </summary>
    LessThan,

    /// <summary>
    /// Состояние полученного `>`. Ожидается `=`
    /// </summary>
    GreaterThan,
}
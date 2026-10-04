namespace SimpleSharpLexer;

/// <summary>
/// Типы токенов языка SimpleSharp
/// </summary>
public enum TokenType
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    Identifier,

    /// <summary>
    /// Целочисленный литерал
    /// </summary>
    Int,

    /// <summary>
    /// Ключевое слово bool
    /// </summary>
    Bool,

    /// <summary>
    /// Ключевое слово string
    /// </summary>
    String,

    /// <summary>
    /// Ключевое слово char
    /// </summary>
    Char,

    /// <summary>
    /// Ключевое слово uint
    /// </summary>
    Uint,

    /// <summary>
    /// Ключевое слово num
    /// </summary>
    Num,

    /// <summary>
    /// Ключевое слово true
    /// </summary>
    True,

    /// <summary>
    /// Ключевое слово false
    /// </summary>
    False,

    /// <summary>
    /// Ключевое слово struct
    /// </summary>
    Struct,

    /// <summary>
    /// Ключевое слово void
    /// </summary>
    Void,

    /// <summary>
    /// Ключевове слово if
    /// </summary>
    If,

    /// <summary>
    /// Ключевое слово else
    /// </summary>
    Else,

    /// <summary>
    /// Ключевое слово while
    /// </summary>
    While,

    /// <summary>
    /// Ключевое слово return
    /// </summary>
    Return,

    /// <summary>
    /// Ключевое слово break
    /// </summary>
    Break,

    /// <summary>
    /// Ключевое слово continue
    /// </summary>
    Continue,

    /// <summary>
    /// Целочисленный литерал
    /// </summary>
    IntLiteral,

    /// <summary>
    /// Строковый литерал
    /// </summary>
    StringLiteral,

    /// <summary>
    /// Символьный литерал
    /// </summary>
    CharLiteral,

    /// <summary>
    /// Оператор сложения "+"
    /// </summary>
    Plus,

    /// <summary>
    /// Оператор вычитания "-"
    /// </summary>
    Minus,

    /// <summary>
    /// Оператор умножения "*"
    /// </summary>
    Multiply,

    /// <summary>
    /// Оператор деления "/"
    /// </summary>
    Divide,

    /// <summary>
    /// Логический оператор "не"
    /// </summary>
    Not,

    /// <summary>
    /// Логический оператор "и"
    /// </summary>
    And,

    /// <summary>
    /// Логический оператор "или"
    /// </summary>
    Or,

    /// <summary>
    /// Оператор присвоить "="
    /// </summary>
    Assign,

    /// <summary>
    /// Оператор сравнения "равно"
    /// </summary>
    Equals,

    /// <summary>
    /// Оператор сравнения "не равно"
    /// </summary>
    NotEquals,

    /// <summary>
    /// Оператор сравнения "меньше"
    /// </summary>
    Less,

    /// <summary>
    /// Оператор сравнения "больше"
    /// </summary>
    Greater,

    /// <summary>
    /// Оператор сравнения "меньше или равно"
    /// </summary>
    LessOrEquals,

    /// <summary>
    /// Оператор сравнения "больше или равно"
    /// </summary>
    GreaterOrEquals,

    /// <summary>
    /// Открывающая круглая скобка
    /// </summary>
    OpenParenthesis,

    /// <summary>
    /// Закрывающая круглая скобка
    /// </summary>
    CloseParenthesis,

    /// <summary>
    /// Открывающая квадратная скобка
    /// </summary>
    OpenBracket,

    /// <summary>
    /// Закрывающая квадратная скобка
    /// </summary>
    CloseBracket,

    /// <summary>
    /// Открывающая фигурная скобка
    /// </summary>
    OpenBrace,

    /// <summary>
    /// Закрывающая фигурная скобка
    /// </summary>
    CloseBrace,

    /// <summary>
    /// Точка
    /// </summary>
    Point,

    /// <summary>
    /// Запятая
    /// </summary>
    Comma,

    /// <summary>
    /// Точка с запятой
    /// </summary>
    Semicolon,

    /// <summary>
    /// Недопустимая лексема
    /// </summary>
    Error,
}
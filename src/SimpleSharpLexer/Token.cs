namespace SimpleSharpLexer;

/// <summary>
/// Токен, представляющий одну лексему языка SimpleSharp
/// </summary>
public struct Token
{
    public Token(TokenType type, string? value = null)
    {
        Value = value;
        Type = type;
    }

    public string? Value { get; private init; }

    public TokenType Type { get; private init; }

    public required int Line { get; init; }

    public required int Column { get; init; }
}
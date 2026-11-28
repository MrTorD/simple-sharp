using Lexer.Abstractions;
using Lexer.Commands;

namespace Lexer;

public static class LexerCommandsExtensions
{
    public static LexerCommandBuilder<TTokenType> AppendCurrentChar<TTokenType>(
        this LexerCommandBuilder<TTokenType> builder)
        where TTokenType : struct, Enum
    {
        return builder.AddCommand(new AppendCurrentCharCommand());
    }

    public static LexerCommandBuilder<TTokenType> PopToken<TTokenType>(
        this LexerCommandBuilder<TTokenType> builder,
        TTokenType tokenType)
        where TTokenType : struct, Enum
    {
        return builder.AddCommand(new PopTokenCommand<TTokenType>(tokenType));
    }

    public static LexerCommandBuilder<TTokenType> AdvanceColumn<TTokenType>(
        this LexerCommandBuilder<TTokenType> builder,
        int column = 1)
        where TTokenType : struct, Enum
    {
        return builder.AddCommand(new AdvanceColumnCommand(column));
    }

    public static LexerCommandBuilder<TTokenType> NewLine<TTokenType>(
        this LexerCommandBuilder<TTokenType> builder)
        where TTokenType : struct, Enum
    {
        return builder.AddCommand(new NewLineCommand());
    }

    public static LexerCommandBuilder<TTokenType> AppendEscapeChar<TTokenType>(
        this LexerCommandBuilder<TTokenType> builder,
        Dictionary<char, char> escapeChars)
        where TTokenType : struct, Enum
    {
        return builder.AddCommand(new AppendEscapeChar(escapeChars));
    }

    public static LexerCommandBuilder<TTokenType> PopDictionaryToken<TTokenType>(
        this LexerCommandBuilder<TTokenType> builder,
        Dictionary<string, TTokenType> tokenTypes,
        TTokenType failureToken)
        where TTokenType : struct, Enum
    {
        return builder.AddCommand(new PopDictionaryTokenCommand<TTokenType>(tokenTypes, failureToken));
    }

    public static LexerCommandBuilder<TTokenType> PopValidatedToken<TTokenType>(
        this LexerCommandBuilder<TTokenType> builder,
        Func<string, bool> validation,
        TTokenType successToken,
        TTokenType failureToken)
        where TTokenType : struct, Enum
    {
        return builder.AddCommand(new PopValidatedTokenCommand<TTokenType>(validation, successToken, failureToken));
    }
}
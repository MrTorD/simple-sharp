using Lexer.Abstractions;
using Lexer.Commands;

namespace Lexer;

public static class LexerCommandsExtensions
{
    public static LexerCommandBuilder<TTokenType> AppendCurrentChar<TTokenType>(
        this LexerCommandBuilder<TTokenType> builder)
        where TTokenType : struct, Enum
    {
        builder.AddCommand(new AppendCurrentCharCommand());
        return builder;
    }

    public static LexerCommandBuilder<TTokenType> PopToken<TTokenType>(
        this LexerCommandBuilder<TTokenType> builder,
        TTokenType tokenType)
        where TTokenType : struct, Enum
    {
        builder.AddCommand(new PopTokenCommand<TTokenType>(tokenType));
        return builder;
    }

    public static LexerCommandBuilder<TTokenType> AdvanceColumn<TTokenType>(
        this LexerCommandBuilder<TTokenType> builder,
        int column = 1)
        where TTokenType : struct, Enum
    {
        builder.AddCommand(new AdvanceColumnCommand(column));
        return builder;
    }

    public static LexerCommandBuilder<TTokenType> NewLine<TTokenType>(
        this LexerCommandBuilder<TTokenType> builder)
        where TTokenType : struct, Enum
    {
        builder.AddCommand(new NewLineCommand());
        return builder;
    }

    public static LexerCommandBuilder<TTokenType> AppendEscapeChar<TTokenType>(
        this LexerCommandBuilder<TTokenType> builder,
        Dictionary<char, char> escapeChars)
        where TTokenType : struct, Enum
    {
        builder.AddCommand(new AppendEscapeChar(escapeChars));
        return builder;
    }

    public static LexerCommandBuilder<TTokenType> PopDictionaryToken<TTokenType>(
        this LexerCommandBuilder<TTokenType> builder,
        Dictionary<string, TTokenType> tokenTypes,
        TTokenType failureToken)
        where TTokenType : struct, Enum
    {
        builder.AddCommand(new PopDictionaryTokenCommand<TTokenType>(tokenTypes, failureToken));
        return builder;
    }
}
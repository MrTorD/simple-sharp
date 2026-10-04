using Lexer;
using Lexer.Abstractions;

namespace SimpleSharpLexer;

/// <summary>
/// Правила переходов и создания ошибок для конечного автомата, реализующего лексер языка SimpleSharp
/// </summary>
internal static class SimpleSharpRules
{
    /// <summary>
    /// Поддерживаемые escape-последовательности
    /// </summary>
    public static readonly Dictionary<char, char> EscapeCharacters = new()
    {
        ['n'] = '\n',
        ['t'] = '\t',
        ['r'] = '\r',
        ['\\'] = '\\',
        ['\''] = '\'',
        ['"'] = '"',
    };

    /// <summary>
    /// Поддерживаемые разделители
    /// </summary>
    public static readonly Dictionary<string, TokenType> Dividers = new()
    {
        [";"] = TokenType.Semicolon,
        ["."] = TokenType.Point,
        [","] = TokenType.Comma,
        ["("] = TokenType.OpenParenthesis,
        [")"] = TokenType.CloseParenthesis,
        ["["] = TokenType.OpenBracket,
        ["]"] = TokenType.CloseBracket,
        ["{"] = TokenType.OpenBrace,
        ["}"] = TokenType.CloseBrace,
        ["+"] = TokenType.Plus,
        ["-"] = TokenType.Minus,
        ["*"] = TokenType.Multiply,
        ["/"] = TokenType.Divide,
    };

    /// <summary>
    /// Ключевые слова
    /// </summary>
    public static readonly Dictionary<string, TokenType> Keywords = new()
    {
        ["if"] = TokenType.If,
        ["else"] = TokenType.Else,
        ["while"] = TokenType.While,
        ["int"] = TokenType.Int,
        ["num"] = TokenType.Num,
        ["uint"] = TokenType.Uint,
        ["bool"] = TokenType.Bool,
        ["char"] = TokenType.Char,
        ["string"] = TokenType.String,
        ["return"] = TokenType.Return,
        ["break"] = TokenType.Break,
        ["continue"] = TokenType.Continue,
        ["struct"] = TokenType.Struct,
        ["void"] = TokenType.Void,
        ["true"] = TokenType.True,
        ["false"] = TokenType.False,
    };

    /// <summary>
    /// Правила переходов для каждого состояния
    /// </summary>
    public static readonly Dictionary<LexerState, IReadOnlyCollection<TransitionRule<LexerState, TokenType>>>
        TransitionRules =
            new()
            {
                [LexerState.Main] = GetMainStateRules(_ => { }),
                [LexerState.IdentifierOrKeyword] = GetIdentifierOrKeywordRules(),
                [LexerState.StringLiteral] = GetStringLiteralRules(),
                [LexerState.StringEscapeCharacter] = GetStringEscapeCharacterRules(),
                [LexerState.CharLiteral] = GetCharLiteralRules(),
                [LexerState.CharReceived] = GetCharReceivedRules(),
                [LexerState.CharEscapeCharacter] = GetCharEscapeCharacterRules(),
                [LexerState.IntLiteral] = GetIntLiteralRules(),
                [LexerState.SlashReceived] = GetSlashRules(),
                [LexerState.CarriageReturnReceived] = GetCarriageReturnRules(),
                [LexerState.Comment] = GetCommentRules(),
                [LexerState.Divider] = GetDividerRules(),
                [LexerState.Or] = GetLogicalOperatorRules('|', TokenType.Or),
                [LexerState.And] = GetLogicalOperatorRules('&', TokenType.And),
                [LexerState.Exclamation] = GetComparisonOperatorRules(TokenType.NotEquals, TokenType.Not),
                [LexerState.Equals] = GetComparisonOperatorRules(TokenType.Equals, TokenType.Assign),
                [LexerState.LessThan] = GetComparisonOperatorRules(TokenType.LessOrEquals, TokenType.Less),
                [LexerState.GreaterThan] = GetComparisonOperatorRules(TokenType.GreaterOrEquals, TokenType.Greater),
            };

    /// <summary>
    /// Правила генерации ошибок, при встрече неожиданного символа
    /// </summary>
    public static readonly Func<LexerState, char, (TokenType Type, string Message)> ErrorFunc
        = (state, c) => state switch
        {
            LexerState.StringLiteral => (TokenType.Error, "Ожидался символ \""),
            LexerState.StringEscapeCharacter => (TokenType.Error, $"Неизвестная ESCAPE-последовательность для '{c}'"),
            LexerState.CharLiteral => (TokenType.Error, "Символьный литерал не может быть пустым"),
            LexerState.CharReceived => (TokenType.Error, "Ожидался символ '"),
            LexerState.CharEscapeCharacter => (TokenType.Error, $"Неизвестная ESCAPE-последовательность для '{c}'"),
            _ => (TokenType.Error, $"Встретился неожиданный символ '{c}'"),
        };

    private static List<TransitionRule<LexerState, TokenType>> GetMainStateRules(
        Action<LexerCommandBuilder<TokenType>> transitionRules) =>
    [
        new()
        {
            Condition = c => c == '_' || char.IsAsciiLetter(c),
            Transition = new(
                LexerState.IdentifierOrKeyword,
                builder =>
                {
                    transitionRules.Invoke(builder);
                    builder.AppendCurrentChar();
                    builder.AdvanceColumn();
                }),
        },
        new()
        {
            Condition = char.IsAsciiDigit,
            Transition = new(
                LexerState.IntLiteral,
                builder =>
                {
                    transitionRules.Invoke(builder);
                    builder.AppendCurrentChar();
                    builder.AdvanceColumn();
                }),
        },
        new()
        {
            Condition = c => c == '"',
            Transition = new(
                LexerState.StringLiteral,
                builder =>
                {
                    transitionRules.Invoke(builder);
                    builder.AdvanceColumn();
                }),
        },
        new()
        {
            Condition = c => c == '\'',
            Transition = new(
                LexerState.CharLiteral,
                builder =>
                {
                    transitionRules.Invoke(builder);
                    builder.AdvanceColumn();
                }),
        },
        new()
        {
            Condition = c => c == '/',
            Transition = new(
                LexerState.SlashReceived,
                builder =>
                {
                    transitionRules.Invoke(builder);
                    builder.AdvanceColumn();
                }),
        },
        new()
        {
            Condition = c => Dividers.ContainsKey(c.ToString()),
            Transition = new(
                LexerState.Divider,
                builder =>
                {
                    transitionRules.Invoke(builder);
                    builder.AppendCurrentChar();
                    builder.AdvanceColumn();
                }),
        },
        new()
        {
            Condition = c => c == '|',
            Transition = new(
                LexerState.Or,
                builder =>
                {
                    transitionRules.Invoke(builder);
                    builder.AdvanceColumn();
                }),
        },
        new()
        {
            Condition = c => c == '&',
            Transition = new(
                LexerState.And,
                builder =>
                {
                    transitionRules.Invoke(builder);
                    builder.AdvanceColumn();
                }),
        },
        new()
        {
            Condition = c => c == '!',
            Transition = new(
                LexerState.Exclamation,
                builder =>
                {
                    transitionRules.Invoke(builder);
                    builder.AdvanceColumn();
                }),
        },
        new()
        {
            Condition = c => c == '=',
            Transition = new(
                LexerState.Equals,
                builder =>
                {
                    transitionRules.Invoke(builder);
                    builder.AdvanceColumn();
                }),
        },
        new()
        {
            Condition = c => c == '<',
            Transition = new(
                LexerState.LessThan,
                builder =>
                {
                    transitionRules.Invoke(builder);
                    builder.AdvanceColumn();
                }),
        },
        new()
        {
            Condition = c => c == '>',
            Transition = new(
                LexerState.GreaterThan,
                builder =>
                {
                    transitionRules.Invoke(builder);
                    builder.AdvanceColumn();
                }),
        },
        new()
        {
            Condition = c => c == '\r',
            Transition = new(
                LexerState.CarriageReturnReceived,
                transitionRules.Invoke),
        },
        new()
        {
            Condition = c => c == '\n',
            Transition = new(
                LexerState.Main,
                builder =>
                {
                    transitionRules.Invoke(builder);
                    builder.NewLine();
                }),
        },
        new()
        {
            Condition = IsWhiteSpace,
            Transition = new(
                LexerState.Main,
                builder =>
                {
                    transitionRules.Invoke(builder);
                    builder.AdvanceColumn();
                }),
        },
    ];

    private static List<TransitionRule<LexerState, TokenType>> GetIntLiteralRules()
    {
        return GetMainStateRules(b => b
                .PopToken(TokenType.IntLiteral))
            .Prepend(new()
            {
                Condition = char.IsAsciiDigit,
                Transition = new(
                    LexerState.IntLiteral,
                    builder => builder
                        .AppendCurrentChar()
                        .AdvanceColumn()),
            })
            .ToList();
    }

    private static List<TransitionRule<LexerState, TokenType>> GetCharLiteralRules() =>
    [
        new()
        {
            Condition = c => c == '\\',
            Transition = new(
                LexerState.CharEscapeCharacter,
                builder => builder.AdvanceColumn()),
        },
        new()
        {
            Condition = c => c != '\'' && c != '\n' && c != '\r',
            Transition = new(
                LexerState.CharReceived,
                builder => builder
                    .AppendCurrentChar()
                    .AdvanceColumn()),
        },
    ];

    private static List<TransitionRule<LexerState, TokenType>> GetCharEscapeCharacterRules() =>
    [
        new()
        {
            Condition = EscapeCharacters.ContainsKey,
            Transition = new(
                LexerState.CharReceived,
                builder => builder
                    .AppendEscapeChar(EscapeCharacters)
                    .AdvanceColumn()),
        }
    ];

    private static List<TransitionRule<LexerState, TokenType>> GetCharReceivedRules() =>
    [
        new()
        {
            Condition = c => c == '\'',
            Transition = new(
                LexerState.Main,
                builder => builder
                    .AdvanceColumn()
                    .PopToken(TokenType.CharLiteral)),
        },
    ];

    private static List<TransitionRule<LexerState, TokenType>> GetStringLiteralRules() =>
    [
        new()
        {
            Condition = c => c == '\\',
            Transition = new(
                LexerState.StringEscapeCharacter,
                builder => builder
                    .AdvanceColumn()),
        },
        new()
        {
            Condition = c => c == '"',
            Transition = new(
                LexerState.Main,
                builder => builder
                    .PopToken(TokenType.StringLiteral)
                    .AdvanceColumn()),
        },
        new()
        {
            Condition = c => c != '\n' && c != '\r',
            Transition = new(
                LexerState.StringLiteral,
                builder => builder
                    .AppendCurrentChar()
                    .AdvanceColumn()),
        },
    ];

    private static List<TransitionRule<LexerState, TokenType>> GetStringEscapeCharacterRules() =>
    [
        new()
        {
            Condition = EscapeCharacters.ContainsKey,
            Transition = new(
                LexerState.StringLiteral,
                builder => builder
                    .AppendEscapeChar(EscapeCharacters)
                    .AdvanceColumn()),
        },
    ];

    private static List<TransitionRule<LexerState, TokenType>> GetIdentifierOrKeywordRules() =>
        GetMainStateRules(b => b
                .PopDictionaryToken(Keywords, TokenType.Identifier))
            .Prepend(new()
            {
                Condition = c => c == '_' || char.IsAsciiLetter(c) || char.IsAsciiDigit(c),
                Transition = new(
                    LexerState.IdentifierOrKeyword,
                    builder => builder
                        .AppendCurrentChar()
                        .AdvanceColumn()),
            })
            .ToList();

    private static List<TransitionRule<LexerState, TokenType>> GetSlashRules() =>
        GetMainStateRules(b => b
                .PopToken(TokenType.Divide))
            .Prepend(new()
            {
                Condition = c => c == '/',
                Transition = new(
                    LexerState.Comment,
                    builder => builder
                        .AdvanceColumn()),
            })
            .ToList();

    private static List<TransitionRule<LexerState, TokenType>> GetCarriageReturnRules() =>
        GetMainStateRules(b => b
                .NewLine())
            .Prepend(new()
            {
                Condition = c => c == '\n',
                Transition = new(
                    LexerState.Main,
                    builder => builder
                        .AdvanceColumn()),
            })
            .ToList();

    private static List<TransitionRule<LexerState, TokenType>> GetCommentRules() =>
    [
        new()
        {
            Condition = c => c == '\n',
            Transition = new(
                LexerState.Main,
                builder => builder
                    .NewLine()),
        },
        new()
        {
            Condition = _ => true,
            Transition = new(
                LexerState.Comment,
                builder => builder
                    .AdvanceColumn()),
        },
    ];

    private static List<TransitionRule<LexerState, TokenType>> GetComparisonOperatorRules(
        TokenType successToken,
        TokenType failureToken) =>
        GetMainStateRules(b => b.PopToken(failureToken))
            .Prepend(new()
            {
                Condition = c => c == '=',
                Transition = new(
                    LexerState.Main,
                    builder => builder
                        .AdvanceColumn()
                        .PopToken(successToken)),
            })
            .ToList();

    private static List<TransitionRule<LexerState, TokenType>> GetDividerRules() =>
        GetMainStateRules(b => b
            .PopDictionaryToken(Dividers, TokenType.Error));

    private static List<TransitionRule<LexerState, TokenType>> GetLogicalOperatorRules(char ch, TokenType tokenType) =>
    [
        new()
        {
            Condition = c => c == ch,
            Transition = new(
                LexerState.Main,
                builder => builder
                    .AdvanceColumn()
                    .PopToken(tokenType)),
        },
    ];

    private static bool IsWhiteSpace(char c)
    {
        return c is ' ' or '\n' or '\r' or '\t';
    }
}
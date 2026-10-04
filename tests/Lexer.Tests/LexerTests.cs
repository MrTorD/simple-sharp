using System.Text;

using SimpleSharpLexer;

namespace Lexer.Tests;

public class LexerTests
{
    [Theory]
    [MemberData(nameof(GetIdentifiersAndKeywordsData))]
    [MemberData(nameof(GetIntLiterals))]
    [MemberData(nameof(GetCharLiteralsData))]
    [MemberData(nameof(GetStringLiteralsData))]
    [MemberData(nameof(GetWhitespacesAndCommentsData))]
    [MemberData(nameof(GetPunctuationData))]
    public void TokenizeLexemes(string code, List<Token> expected)
    {
        SimpleSharpLexer.SimpleSharpLexer lexer = new();
        Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(code));
        List<Token> tokens = lexer.ParseSourceCode(stream).ToList();

        Assert.Equal(expected, tokens);
    }

    public static TheoryData<string, List<Token>> GetIdentifiersAndKeywordsData()
    {
        return new TheoryData<string, List<Token>>
        {
            {
                " bool string char int uint num struct void ", [
                    new Token(TokenType.Bool) { Line = 1, Column = 6 },
                    new Token(TokenType.String) { Line = 1, Column = 13 },
                    new Token(TokenType.Char) { Line = 1, Column = 18 },
                    new Token(TokenType.Int) { Line = 1, Column = 22 },
                    new Token(TokenType.Uint) { Line = 1, Column = 27 },
                    new Token(TokenType.Num) { Line = 1, Column = 31 },
                    new Token(TokenType.Struct) { Line = 1, Column = 38 },
                    new Token(TokenType.Void) { Line = 1, Column = 43 },
                ]
            },
            {
                "true false ", [
                    new Token(TokenType.True) { Line = 1, Column = 5 },
                    new Token(TokenType.False) { Line = 1, Column = 11 },
                ]
            },
            {
                "if else while break continue return ", [
                    new Token(TokenType.If) { Line = 1, Column = 3 },
                    new Token(TokenType.Else) { Line = 1, Column = 8 },
                    new Token(TokenType.While) { Line = 1, Column = 14 },
                    new Token(TokenType.Break) { Line = 1, Column = 20 },
                    new Token(TokenType.Continue) { Line = 1, Column = 29 },
                    new Token(TokenType.Return) { Line = 1, Column = 36 },
                ]
            },
            {
                "hello h42 ab_44 ", [
                    new Token(TokenType.Identifier, "hello") { Line = 1, Column = 6 },
                    new Token(TokenType.Identifier, "h42") { Line = 1, Column = 10 },
                    new Token(TokenType.Identifier, "ab_44") { Line = 1, Column = 16 },
                ]
            },
            {
                "IF stRing ", [
                    new Token(TokenType.Identifier, "IF") { Line = 1, Column = 3 },
                    new Token(TokenType.Identifier, "stRing") { Line = 1, Column = 10 },
                ]
            },
            {
                "_private_Field ", [
                    new Token(TokenType.Identifier, "_private_Field") { Line = 1, Column = 15 },
                ]
            },
            {
                "ЕСЛИ 5 > 4 ", [
                    new Token(TokenType.Error, "Встретился неожиданный символ 'Е'") { Line = 1, Column = 1 },
                ]
            },
        };
    }

    public static TheoryData<string, List<Token>> GetIntLiterals()
    {
        return new TheoryData<string, List<Token>>
        {
            {
                " 12 0 0011 ", [
                    new Token(TokenType.IntLiteral, "12") { Line = 1, Column = 4 },
                    new Token(TokenType.IntLiteral, "0") { Line = 1, Column = 6 },
                    new Token(TokenType.IntLiteral, "0011") { Line = 1, Column = 11 },
                ]
            },
            {
                " +44 -77 ", [
                    new Token(TokenType.Plus) { Line = 1, Column = 3 },
                    new Token(TokenType.IntLiteral, "44") { Line = 1, Column = 5 },
                    new Token(TokenType.Minus) { Line = 1, Column = 7 },
                    new Token(TokenType.IntLiteral, "77") { Line = 1, Column = 9 },
                ]
            },
        };
    }

    public static TheoryData<string, List<Token>> GetCharLiteralsData()
    {
        return new TheoryData<string, List<Token>>
        {
            {
                @" 'c' '\t' '\'' ", [
                    new Token(TokenType.CharLiteral, "c") { Line = 1, Column = 5 },
                    new Token(TokenType.CharLiteral, "\t") { Line = 1, Column = 10 },
                    new Token(TokenType.CharLiteral, "'") { Line = 1, Column = 15 },
                ]
            },
            {
                " 'a ", [
                    new Token(TokenType.Error, "Ожидался символ '") { Line = 1, Column = 4 },
                ]
            },
            {
                " '' ", [
                    new Token(TokenType.Error, "Символьный литерал не может быть пустым") { Line = 1, Column = 3 },
                ]
            },
            {
                " 'ab' ", [
                    new Token(TokenType.Error, "Ожидался символ '") { Line = 1, Column = 4 },
                ]
            },
            {
                @" '\r\'' ", [
                    new Token(TokenType.Error, "Ожидался символ '") { Line = 1, Column = 5 },
                ]
            },
            {
                @" '\g' ", [
                    new Token(TokenType.Error, "Неизвестная ESCAPE-последовательность для 'g'")
                        {
                            Line = 1, Column = 4,
                        },
                ]
            },
            {
                " 'я' ", [
                    new Token(TokenType.CharLiteral, "я") { Line = 1, Column = 5, },
                ]
            },
        };
    }

    public static TheoryData<string, List<Token>> GetStringLiteralsData()
    {
        return new TheoryData<string, List<Token>>
        {
            {
                """   ""    "a"   "Hello, World"  """, [
                    new Token(TokenType.StringLiteral) { Line = 1, Column = 5 },
                    new Token(TokenType.StringLiteral, "a") { Line = 1, Column = 12 },
                    new Token(TokenType.StringLiteral, "Hello, World") { Line = 1, Column = 29 },
                ]
            },
            {
                """ "Привет, мир!" """, [
                    new Token(TokenType.StringLiteral, "Привет, мир!") { Line = 1, Column = 15 },
                ]
            },
            {
                """ "Hello, \"User\""  "Bye!\n"  """, [
                    new Token(TokenType.StringLiteral, "Hello, \"User\"") { Line = 1, Column = 18 },
                    new Token(TokenType.StringLiteral, "Bye!\n") { Line = 1, Column = 28 },
                ]
            },
            {
                """
                 "Hello
                 
                """,
                [
                    new Token(TokenType.Error, "Ожидался символ \"") { Line = 1, Column = 8 },
                ]
            },
            {
                """ "\p"  """, [
                    new Token(TokenType.Error, "Неизвестная ESCAPE-последовательность для 'p'")
                        {
                            Line = 1, Column = 4,
                        },
                ]
            },
        };
    }

    public static TheoryData<string, List<Token>> GetWhitespacesAndCommentsData()
    {
        return new TheoryData<string, List<Token>>
        {
            {
                "  \n     \r    \t\t\t   ", [
                ]
            },
            {
                " //Hello, World   ", [
                ]
            },
        };
    }

    public static TheoryData<string, List<Token>> GetPunctuationData()
    {
        return new TheoryData<string, List<Token>>
        {
            {
                " x + y - a * b / 4 ", [
                    new Token(TokenType.Identifier, "x") { Line = 1, Column = 3 },
                    new Token(TokenType.Plus) { Line = 1, Column = 5 },
                    new Token(TokenType.Identifier, "y") { Line = 1, Column = 7 },
                    new Token(TokenType.Minus) { Line = 1, Column = 9 },
                    new Token(TokenType.Identifier, "a") { Line = 1, Column = 11 },
                    new Token(TokenType.Multiply) { Line = 1, Column = 13 },
                    new Token(TokenType.Identifier, "b") { Line = 1, Column = 15 },
                    new Token(TokenType.Divide) { Line = 1, Column = 17 },
                    new Token(TokenType.IntLiteral, "4") { Line = 1, Column = 19 },
                ]
            },
            {
                " x > 4 && y < 7 || a == b || c != b ", [
                    new Token(TokenType.Identifier, "x") { Line = 1, Column = 3 },
                    new Token(TokenType.Greater) { Line = 1, Column = 5 },
                    new Token(TokenType.IntLiteral, "4") { Line = 1, Column = 7 },
                    new Token(TokenType.And) { Line = 1, Column = 10 },
                    new Token(TokenType.Identifier, "y") { Line = 1, Column = 12 },
                    new Token(TokenType.Less) { Line = 1, Column = 14 },
                    new Token(TokenType.IntLiteral, "7") { Line = 1, Column = 16 },
                    new Token(TokenType.Or) { Line = 1, Column = 19 },
                    new Token(TokenType.Identifier, "a") { Line = 1, Column = 21 },
                    new Token(TokenType.Equals) { Line = 1, Column = 24 },
                    new Token(TokenType.Identifier, "b") { Line = 1, Column = 26 },
                    new Token(TokenType.Or) { Line = 1, Column = 29 },
                    new Token(TokenType.Identifier, "c") { Line = 1, Column = 31 },
                    new Token(TokenType.NotEquals) { Line = 1, Column = 34 },
                    new Token(TokenType.Identifier, "b") { Line = 1, Column = 36 },
                ]
            },
            {
                " !(x >= y) || b <= 2 ", [
                    new Token(TokenType.Not) { Line = 1, Column = 3 },
                    new Token(TokenType.OpenParenthesis) { Line = 1, Column = 4 },
                    new Token(TokenType.Identifier, "x") { Line = 1, Column = 5 },
                    new Token(TokenType.GreaterOrEquals) { Line = 1, Column = 8 },
                    new Token(TokenType.Identifier, "y") { Line = 1, Column = 10 },
                    new Token(TokenType.CloseParenthesis) { Line = 1, Column = 11 },
                    new Token(TokenType.Or) { Line = 1, Column = 14 },
                    new Token(TokenType.Identifier, "b") { Line = 1, Column = 16 },
                    new Token(TokenType.LessOrEquals) { Line = 1, Column = 19 },
                    new Token(TokenType.IntLiteral, "2") { Line = 1, Column = 21 },
                ]
            },
            {
                " if (arr[5] < 2){} ", [
                    new Token(TokenType.If) { Line = 1, Column = 4 },
                    new Token(TokenType.OpenParenthesis) { Line = 1, Column = 6 },
                    new Token(TokenType.Identifier, "arr") { Line = 1, Column = 9 },
                    new Token(TokenType.OpenBracket) { Line = 1, Column = 10 },
                    new Token(TokenType.IntLiteral, "5") { Line = 1, Column = 11 },
                    new Token(TokenType.CloseBracket) { Line = 1, Column = 12 },
                    new Token(TokenType.Less) { Line = 1, Column = 14 },
                    new Token(TokenType.IntLiteral, "2") { Line = 1, Column = 16 },
                    new Token(TokenType.CloseParenthesis) { Line = 1, Column = 17 },
                    new Token(TokenType.OpenBrace) { Line = 1, Column = 18 },
                    new Token(TokenType.CloseBrace) { Line = 1, Column = 19 },
                ]
            },
            {
                " point.x = 7 ", [
                    new Token(TokenType.Identifier, "point") { Line = 1, Column = 7 },
                    new Token(TokenType.Point) { Line = 1, Column = 8 },
                    new Token(TokenType.Identifier, "x") { Line = 1, Column = 9 },
                    new Token(TokenType.Assign) { Line = 1, Column = 11 },
                    new Token(TokenType.IntLiteral, "7") { Line = 1, Column = 13 },
                ]
            },
            {
                " foo(5, 4); ", [
                    new Token(TokenType.Identifier, "foo") { Line = 1, Column = 5 },
                    new Token(TokenType.OpenParenthesis) { Line = 1, Column = 6 },
                    new Token(TokenType.IntLiteral, "5") { Line = 1, Column = 7 },
                    new Token(TokenType.Comma) { Line = 1, Column = 8 },
                    new Token(TokenType.IntLiteral, "4") { Line = 1, Column = 10 },
                    new Token(TokenType.CloseParenthesis) { Line = 1, Column = 11 },
                    new Token(TokenType.Semicolon) { Line = 1, Column = 12 },
                ]
            },
        };
    }
}
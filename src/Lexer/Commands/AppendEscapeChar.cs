using Lexer.Abstractions;

namespace Lexer.Commands;

public class AppendEscapeChar : ILexerCommand
{
    private Dictionary<char, char> _escapeDictionary;

    public AppendEscapeChar(Dictionary<char, char> escapeDictionary)
    {
        _escapeDictionary = escapeDictionary;
    }

    public void Execute(ILexerCommandContext context)
    {
        context.TokenBuffer.Append(_escapeDictionary[context.CurrentChar]);
    }
}
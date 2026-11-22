using Lexer.Abstractions;

namespace Lexer.Commands;

public class PopDictionaryTokenCommand<TTokenType> : ILexerCommand<TTokenType>
    where TTokenType : struct, Enum
{
    private Dictionary<string, TTokenType> _tokenDictionary;
    private TTokenType _failureToken;

    public PopDictionaryTokenCommand(Dictionary<string, TTokenType> tokenDictionary, TTokenType failureToken)
    {
        _tokenDictionary = tokenDictionary;
        _failureToken = failureToken;
    }

    public void Execute(ILexerCommandContext<TTokenType> context)
    {
        if (!_tokenDictionary.TryGetValue(context.TokenBuffer.ToString(), out TTokenType token))
        {
            context.AddToken(_failureToken);
            context.TokenBuffer.Clear();
            return;
        }

        context.TokenBuffer.Clear();
        context.AddToken(token);
    }
}
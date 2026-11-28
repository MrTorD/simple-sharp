using System.Diagnostics.CodeAnalysis;

using Lexer.Abstractions;

namespace Lexer;

public class TextReader : IFileReader
{
    private readonly string _input;
    private int _position;

    public TextReader(string input)
    {
        _input = input + "\n";
        _position = 0;
    }

    public bool TryNext([NotNullWhen(true)] out char? c)
    {
        if (_position < _input.Length)
        {
            c = _input[_position++];
            return true;
        }

        c = null;
        return false;
    }
}
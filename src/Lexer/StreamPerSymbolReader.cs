using System.Diagnostics.CodeAnalysis;
using System.Text;

using Lexer.Abstractions;

namespace Lexer;

public class StreamPerSymbolReader : IFileReader, IDisposable
{
    private readonly StreamReader _reader;
    private readonly char[] _buffer;
    private int _read;
    private int _position;
    private bool _isLastCharGiven;

    public StreamPerSymbolReader(Stream stream, Encoding encoding, int bufferSize)
    {
        _reader = new StreamReader(
            stream,
            encoding,
            detectEncodingFromByteOrderMarks: true,
            bufferSize: bufferSize,
            leaveOpen: true);
        _buffer = new char[bufferSize];
        _isLastCharGiven = false;
    }

    public bool TryNext([NotNullWhen(true)] out char? c)
    {
        if (_position < _read)
        {
            c = _buffer[_position++];
            return true;
        }

        _read = _reader.Read(_buffer, 0, _buffer.Length);

        if (_isLastCharGiven)
        {
            c = null;
            return false;
        }

        if (_read == 0)
        {
            _isLastCharGiven = true;
            c = '\n';
            return true;
        }

        _position = 0;
        c = _buffer[_position++];
        return true;
    }

    public void Dispose()
    {
        _reader.Dispose();
    }
}
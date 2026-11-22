using System.Text;

using VirtualMachine;

namespace Interpreter.Tests;

public class FakeEnvironment : IEnvironment
{
    private readonly Queue<string> _input;

    private readonly StringBuilder _output = new();

    public FakeEnvironment(IEnumerable<string> input)
    {
        _input = new Queue<string>(input);
    }

    public string Output => _output.ToString();

    public string? ReadLine()
    {
        return _input.Count != 0 ? _input.Dequeue() : null;
    }

    public void Write(int value)
    {
        _output.Append(value);
    }
}
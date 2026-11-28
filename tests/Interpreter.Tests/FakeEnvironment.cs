using System.Text;

using VirtualMachine;

namespace Interpreter.Tests;

public class FakeEnvironment : IEnvironment
{
    private readonly StringBuilder _output = new();

    public string Output => _output.ToString();

    public void Write(int value)
    {
        _output.Append(value);
    }
}
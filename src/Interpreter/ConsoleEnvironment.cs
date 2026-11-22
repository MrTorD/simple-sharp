using VirtualMachine;

namespace Interpreter;

public class ConsoleEnvironment : IEnvironment
{
    public string? ReadLine()
    {
        return Console.ReadLine();
    }

    public void Write(int value)
    {
        Console.WriteLine(value);
    }
}
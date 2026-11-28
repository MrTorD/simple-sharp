using VirtualMachine;

namespace Interpreter;

public class ConsoleEnvironment : IEnvironment
{
    public void Write(int value)
    {
        Console.Write(value);
    }
}
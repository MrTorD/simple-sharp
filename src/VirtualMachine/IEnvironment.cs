namespace VirtualMachine;

public interface IEnvironment
{
    string? ReadLine();

    void Write(int value);
}
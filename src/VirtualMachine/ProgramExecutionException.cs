namespace VirtualMachine;

public class ProgramExecutionException : Exception
{
    public ProgramExecutionException()
    {
    }

    public ProgramExecutionException(string? message)
        : base(message)
    {
    }

    public ProgramExecutionException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
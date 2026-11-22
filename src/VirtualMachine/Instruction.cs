namespace VirtualMachine;

public record struct Instruction
{
    public Instruction(OperationType type, int? operand = null)
    {
        OperationType = type;
        Operand = operand;
    }

    public OperationType OperationType { get; }

    public int? Operand { get; }
}
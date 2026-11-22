using VirtualMachine;

namespace Parser;

public class InstructionsBuilder
{
    private readonly List<Instruction> _instructions = [];

    public void Halt()
    {
        _instructions.Add(new Instruction(OperationType.Halt));
    }

    public void Push(int value)
    {
        _instructions.Add(new Instruction(OperationType.Push, value));
    }

    public void Add()
    {
        _instructions.Add(new Instruction(OperationType.Add));
    }

    public void Subtract()
    {
        _instructions.Add(new Instruction(OperationType.Sub));
    }

    public void Multiply()
    {
        _instructions.Add(new Instruction(OperationType.Mul));
    }

    public void Divide()
    {
        _instructions.Add(new Instruction(OperationType.Div));
    }

    public void DivWithRemainder()
    {
        _instructions.Add(new Instruction(OperationType.DivWithRemainder));
    }

    public void Exponent()
    {
        _instructions.Add(new Instruction(OperationType.Exponent));
    }

    public void Negate()
    {
        _instructions.Add(new Instruction(OperationType.Negate));
    }

    public void Write()
    {
        _instructions.Add(new Instruction(OperationType.Write));
    }

    public List<Instruction> Build()
    {
        return _instructions;
    }
}
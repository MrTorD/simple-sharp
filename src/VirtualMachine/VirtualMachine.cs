namespace VirtualMachine;

public class VirtualMachine
{
    private readonly List<Instruction> _instructions;
    private readonly IEnvironment _environment;
    private readonly Stack<int> _values;
    private int _instructionPointer;

    public VirtualMachine(IEnumerable<Instruction> instructions, IEnvironment environment)
    {
        _environment = environment;
        _instructions = instructions.ToList();
        _values = new Stack<int>();
        _instructionPointer = 0;
    }

    public void RunProgram()
    {
        try
        {
            while (_instructionPointer < _instructions.Count)
            {
                Instruction instruction = _instructions[_instructionPointer++];
                ExecuteInstruction(instruction);
            }
        }
        catch (DivideByZeroException)
        {
            throw new ProgramExecutionException("Деление на 0");
        }
        catch (OverflowException)
        {
            throw new ProgramExecutionException("Переполнение целого числа");
        }
    }

    private void ExecuteInstruction(Instruction instruction)
    {
        switch (instruction.OperationType)
        {
            case OperationType.Halt:
                return;
            case OperationType.Pop:
                _values.Pop();
                break;
            case OperationType.Push:
                _values.Push(
                    instruction.Operand ??
                    throw new ProgramExecutionException(
                        $"Инструкция {_instructionPointer} не содержит необходимого операнда"));
                break;
            case OperationType.Add:
                ExecuteBinaryOp((left, right) => checked(left + right));
                break;
            case OperationType.Sub:
                ExecuteBinaryOp((left, right) => checked(left - right));
                break;
            case OperationType.Mul:
                ExecuteBinaryOp((left, right) => checked(left * right));
                break;
            case OperationType.Div:
                ExecuteBinaryOp((left, right) => left / right);
                break;
            case OperationType.Negate:
                _values.Push(-_values.Pop());
                break;
            case OperationType.Exponent:
                ExecuteBinaryOp((left, right) => (int)Math.Pow(left, right));
                break;
            case OperationType.DivWithRemainder:
                ExecuteBinaryOp((left, right) => left % right);
                break;
            case OperationType.Write:
                _environment.Write(_values.Pop());
                break;
        }
    }

    private void ExecuteBinaryOp(Func<int, int, int> op)
    {
        int right = _values.Pop();
        int left = _values.Pop();
        _values.Push(op.Invoke(left, right));
    }
}
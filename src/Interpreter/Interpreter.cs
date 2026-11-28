using SimpleSharpLexer;

using VirtualMachine;

namespace Interpreter;

public class Interpreter
{
    private readonly IEnvironment _environment;

    public Interpreter(IEnvironment environment)
    {
        _environment = environment;
    }

    public void Execute(string sourceCode)
    {
        SimpleSharpLexer.SimpleSharpLexer lexer = new();
        IReadOnlyList<Token> tokens = lexer.ParseSourceCode(sourceCode);
        Parser.Parser parser = new(tokens);
        VirtualMachine.VirtualMachine vm = new(parser.Compile(), _environment);
        vm.RunProgram();
    }
}
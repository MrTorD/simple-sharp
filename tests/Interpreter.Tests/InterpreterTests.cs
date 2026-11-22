using System.Text;

using Parser;

using VirtualMachine;

namespace Interpreter.Tests;

public class InterpreterTests
{
    [Theory]
    [MemberData(nameof(WellFormedPrograms))]
    public void RunWellFormedProgram(string code, string expected)
    {
        FakeEnvironment environment = new([]);
        Interpreter interpreter = new(environment);
        interpreter.Execute(new MemoryStream(Encoding.UTF8.GetBytes(code)));
        Assert.Equal(expected, environment.Output);
    }

    [Theory]
    [MemberData(nameof(IllFormedPrograms))]
    public void RunIllFormedProgram(string code, string exceptionMessage)
    {
        FakeEnvironment environment = new([]);
        Interpreter interpreter = new(environment);
        SyntaxException exception =
            Assert.Throws<SyntaxException>(() => interpreter.Execute(new MemoryStream(Encoding.UTF8.GetBytes(code))));
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Theory]
    [MemberData(nameof(ProgramsWithRuntimeExceptions))]
    public void RunProgramWithRuntimeError(string code, string exceptionMessage)
    {
        FakeEnvironment environment = new([]);
        Interpreter interpreter = new(environment);
        ProgramExecutionException exception = Assert.Throws<ProgramExecutionException>(() =>
            interpreter.Execute(new MemoryStream(Encoding.UTF8.GetBytes(code))));
        Assert.Equal(exceptionMessage, exception.Message);
    }

    public static TheoryData<string, string> WellFormedPrograms() => new()
    {
        { "void main() { }", "" },
        { "void main() { ; {;{;;}} }", "" },
        { "void main() { Write(5); }", "5" },
        { "void main() { Write(3 + -2); }", "1" },
        { "void main() { Write(1 + 4 * 2 ^ 3); }", "33" },
        { "void main() { Write( (8 / 2 / 2) ^ 2 ^ 2); }", "16" },
        { "void main() { Write( (11 + -(4 % 3)) / 3 ); }", "3" },
    };

    public static TheoryData<string, string> IllFormedPrograms() => new()
    {
        { "Write(5);", "Ожидалась лексема Void, встречена Identifier" },
        { "int main() ", "Ожидалась лексема Void, встречена Int" },
        { "void main() { Write(5) }", "Ожидалась лексема Semicolon, встречена CloseBrace" },
        { "void main() { Write(5);", "Ожидалась лексема CloseBrace" },
        {
            "void main(){ 5 + 5; }", "Ожидалась лексема Identifier со значением Write, встречена IntLiteral со значением 5"
        },
        { "void main() { Write (1 ++ 1) }", "Ожидался IntLiteral или OpenParenthesis, встречен Plus" },
        { "void main() { Write (2147483648) }", "Целочисленный литерал со значением 2147483648 слишком большой" },
    };

    public static TheoryData<string, string> ProgramsWithRuntimeExceptions() => new()
    {
        { "void main() { Write(2147483647 + 1); }", "Переполнение целого числа" },
        { "void main() { Write(-2147483647 - 2); }", "Переполнение целого числа" },
        { "void main() { Write(5 / 0); }", "Деление на 0" },
    };
}
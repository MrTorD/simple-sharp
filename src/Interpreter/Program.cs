using Interpreter;

if (args.Length != 1)
{
    Console.Error.WriteLine("Передайте путь до файла с исходным кодом в качестве единственного аргумента");
    return 1;
}

try
{
    ConsoleEnvironment environment = new();
    Interpreter.Interpreter interpreter = new(environment);

    interpreter.Execute(File.OpenRead(args[0]));

    return 0;
}
catch (Exception e)
{
    Console.Error.WriteLine($"Интерпретатор выбросил исключение: {e.GetType()}: {e.Message}");
    return 1;
}
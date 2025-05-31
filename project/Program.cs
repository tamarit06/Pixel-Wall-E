using System;

class Program
{
    static void Main(string[] args)
    {
        string codigoFuente =
        @"Spawn(0, 0)
        i<-0
        x<-GetActualX()
        GoTo[aqui](x==0)
        i<-2
        aqui";

        Lexer lexer = new Lexer(codigoFuente);
        lexer.Tokenize();

        var parser = new Parser(lexer);
        parser.Parsind();

        var evaluator = new Evaluate();
        evaluator.EvaluateProgram(parser.Nodos);

        Console.WriteLine(">>> Variables almacenadas:");
        foreach (var kvp in evaluator.Memory)
        {
            Console.WriteLine($"  {kvp.Key} = {kvp.Value}");
        }
    }
}

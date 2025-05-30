using System.Data;

class Program
{
    static void Main(string[] args)
    {
        string codigoFuente =
        @"Spawn(3, 4)
x<- GetColorCount (""White"",0, 0 ,2, 2)";

        Lexer lexer = new Lexer(codigoFuente);
        lexer.Tokenize();

        var parser = new Parser(lexer);
        parser.Parsind();

        var evaluator = new Evaluate();

       foreach (var nodo in parser.Nodos)
{
    Console.WriteLine(">>> AST:");
    ASTPrinter.Print(nodo);

    try
    {
        var result = evaluator.evaluate(nodo);

        // Si es expresión, muestra resultado
        if (nodo is Assignment || nodo is BinaryExpression || nodo is UnaryExpression || nodo is Variable || nodo is Number || nodo is GroupingExpr || nodo is FunctionCallNode)
            Console.WriteLine($">>> Resultado: {result}\n");
        else
            Console.WriteLine(">>> Instrucción ejecutada.\n");
    }
    catch (Exception e)
    {
        Console.WriteLine($">>> Error en evaluación: {e.Message}\n");
    }
}

        Console.WriteLine(">>> Variables almacenadas:");
        foreach (var kvp in evaluator.Memory)
        {
            Console.WriteLine($"  {kvp.Key} = {kvp.Value}");
        }
    }
}

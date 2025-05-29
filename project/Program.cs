class Program
{
    static void Main(string[] args)
    {
        string codigoFuente = @" Spawn(0, 0)
            x <- 3+3==4 || 4>3";

        Lexer lexer = new Lexer(codigoFuente);
        lexer.Tokenize();

        var parser = new Parser(lexer);
        parser.Parsind();

        var evaluator = new Evaluate();

        foreach (var nodo in parser.Nodos)
        {
            Console.WriteLine(">>> AST:");
            ASTPrinter.Print(nodo);

            if (nodo is Assignment || nodo is BinaryExpression || nodo is UnaryExpression || nodo is Variable || nodo is Number || nodo is GroupingExpr)
            {
                try
                {
                    var result = evaluator.evaluate(nodo);
                    Console.WriteLine($">>> Resultado: {result}\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine($">>> Error en evaluación: {e.Message}\n");
                }
            }
            else
            {
                Console.WriteLine(">>> Nodo no evaluado (no es una expresión).\n");
            }
        }

        Console.WriteLine(">>> Variables almacenadas:");
        foreach (var kvp in evaluator.Memory)
        {
            Console.WriteLine($"  {kvp.Key} = {kvp.Value}");
        }
    }
}

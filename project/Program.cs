class Program
{
    static void Main(string[] args)
    {
        string codigoFuente =
        @"Spawn(0, 0)
        Color(""Black"")
         n <- 5
         k <- 3 + 3 * 10
         n <- k * 2
        actual_x <- GetActualX()
        i <- 0
        loop1
DrawLine(1, 0, 1)
i <- i + 1
 is_brush_color_blue <- IsBrushColor(""Blue"")
 GoTo [loop_ends_here] (is_brush_color_blue == 1)
GoTo [loop1] (i < 10)
Color(""Blue"")
GoTo [loop1] (1 == 1)
 loop_ends_here"; // tu código aquí

        Lexer lexer = new Lexer(codigoFuente);
        lexer.Tokenize();

        // Imprimir la lista de tokens
        foreach (var token in lexer.Tokens)
        {
            Console.WriteLine(token.ToString());
        }

        var parser = new Parser(lexer);
        parser.Parsind();

        // Imprimir los nodos
        foreach (var nodo in parser.Nodos)
        {
            ASTPrinter.Print(nodo);
        }
    }
}

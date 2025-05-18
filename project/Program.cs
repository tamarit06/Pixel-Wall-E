class Program
{
    static void Main(string[] args)
    {
        string codigoFuente = "x"; // tu código aquí

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

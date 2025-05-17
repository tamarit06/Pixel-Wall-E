class Program
{
    static void Main(string[] args)
    {
        string codigoFuente = "x <- 3 + 5 * 2"; // tu código aquí

        Lexer lexer = new Lexer(codigoFuente);
        lexer.Tokenize();

        Parser parser = new Parser(lexer);
        List<ASTnode> ast = parser.Parse();

        foreach (var nodo in ast)
        {
            ASTPrinter.Print(nodo);
        }
    }
}

class Program
{
    static void Main()
{
      string codigo = @"Color Size DrawCircle mama-mama , ;)";

        Lexer lexer = new Lexer(codigo);
        List<Token> tokens = lexer.Tokenize();

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
}

}


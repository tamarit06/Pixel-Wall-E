public static class ASTPrinter
{
    public static void Print(ASTNode node)
    {
        switch (node)
        {
            case Number n:
                Console.WriteLine($"Number: {n.Value}");
                break;
            case ColorNode c:
                Console.WriteLine($"Color: {c.Value}");
                break;
            case Variable v:
            Console.WriteLine($"Variable: {v.Name}");
             break;
            default:
                Console.WriteLine("Tipo de nodo desconocido");
                break;
        }
    }
}

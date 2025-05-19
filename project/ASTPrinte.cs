public static class ASTPrinter
{
    public static void Print(ASTNode node, string indent = "", bool isLast = true)
    {
        if (node is Number n)
        {
            Console.WriteLine($"{indent}└── Number: {n.Value}");
        }
        else if (node is ColorNode c)
        {
            Console.WriteLine($"{indent}└── Color: {c.Value}");
        }
        else if (node is Variable v)
        {
            Console.WriteLine($"{indent}└── Variable: {v.Name}");
        }
        else if (node is UnaryExpression u)
        {
            Console.WriteLine($"{indent}└── Unary Expression: {u.Operator.Lexeme}");
            Print(u.Right, indent + (isLast ? "    " : "│   "), false);
        }
        else if (node is BinaryExpression b)
        {
            Console.WriteLine($"{indent}└── Binary Expression: {b.Op.Lexeme}");
            Print(b.Left, indent + (isLast ? "    " : "│   "), false);
            Print(b.Right, indent + (isLast ? "    " : "│   "), true);
        }
        else if (node is Assignment a) // Manejo de nodos de asignación
        {
            Console.WriteLine($"{indent}└── Assignment: {a.NameVariable} <-");
            Print(a.Value, indent + (isLast ? "    " : "│   "), true);
        }

         else if (node is FunctionCallNode f)
        {
            Console.WriteLine($"{indent}└── Function Call: {f.FunctionName}");
            for (int i = 0; i < f.Arguments.Count; i++)
            {
                bool lastArg = (i == f.Arguments.Count -1);
                Print(f.Arguments[i], indent + (isLast ? "    " : "│   "), lastArg);
            }
        }
        else
        {
            Console.WriteLine($"{indent}└── Tipo de nodo desconocido");
        }
    }
}

public static class ASTPrinter
{
    public static void Print(ASTNode node, string indent = "", bool isLast = true)
    {
        if (node is Number n)
        {
            Console.WriteLine($"{indent}└── Number: {n.Value}");
        }
        else if (node is StringNode c)
        {
            Console.WriteLine($"{indent}└── String: {c.Value}");
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
         else if (node is GoTo g) // Manejo de nodos de GoTo
        {
            Console.WriteLine($"{indent}└── GoTo: {g.Label}");
            if (g.Condition != null)
            {
                Console.WriteLine($"{indent}    └── Condition:");
                Print(g.Condition, indent + (isLast ? "    " : "│   "), true);
            }
        }
        else if (node is Label l) // Manejo de nodos de Label
        {
            Console.WriteLine($"{indent}└── Label: {l.Value}");
        }

         else if (node is InstructionNode instr) // Nueva sección para instrucciones
        {
            Console.WriteLine($"{indent}└── Instruction: {instr.InstructionName}");
            for (int i = 0; i < instr.Parameters.Count; i++)
            {
                bool lastParam = (i == instr.Parameters.Count - 1);
                Print(instr.Parameters[i], indent + (isLast ? "    " : "│   "), lastParam);
            }
        }
        else
        {
            Console.WriteLine($"{indent}└── Tipo de nodo desconocido");
        }
    }
}

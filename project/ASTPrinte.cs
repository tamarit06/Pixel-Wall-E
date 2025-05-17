public static class ASTPrinter
{
    public static void Print(ASTNode node, string indent = "", bool isLast = true)
    {
        if (node == null) return;

        string marker = isLast ? "└──" : "├──";
        Console.Write(indent);
        Console.Write(marker);

        switch (node)
        {
            case Number n:
                Console.WriteLine($"Number: {n.Token.Lexeme}");
                break;

          //  case VariableReference v:
                Console.WriteLine($"Variable: {v.Name}");
                break;

            case BinaryExpression b:
                Console.WriteLine($"BinaryOp: {b.Operator.Lexeme}");
                Print(b.Left, indent + (isLast ? "    " : "│   "), false);
                Print(b.Right, indent + (isLast ? "    " : "│   "), true);
                break;

            case UnaryExpr u:
                Console.WriteLine($"UnaryOp: {u.Operator.Lexeme}");
                Print(u.Right, indent + (isLast ? "    " : "│   "), true);
                break;

            case Assignment a:
                Console.WriteLine($"Assignment: {a.Name}");
                Print(a.Expression, indent + (isLast ? "    " : "│   "), true);
                break;

            case CallFunction f:
                Console.WriteLine($"CallFunction: {f.Callee.Lexeme}()");
                for (int i = 0; i < f.Arguments.Count; i++)
                    Print(f.Arguments[i], indent + (isLast ? "    " : "│   "), i == f.Arguments.Count - 1);
                break;

            case IfStmt i:
                Console.WriteLine("If");
                Print(i.Condition, indent + (isLast ? "    " : "│   "), false);
                Print(i.ThenBranch, indent + (isLast ? "    " : "│   "), false);
                Print(i.ElseBranch, indent + (isLast ? "    " : "│   "), true);
                break;

            case LetStmt l:
                Console.WriteLine("Let");
                for (int i = 0; i < l.Declarations.Count; i++)
                {
                    var d = l.Declarations[i];
                    Console.Write(indent + (isLast ? "    " : "│   ") + (i == l.Declarations.Count - 1 ? "└──" : "├──"));
                    Console.WriteLine($"Assignment: {d.Name}");
                    Print(d.Expression, indent + (isLast ? "    " : "│   ") + (i == l.Declarations.Count - 1 ? "    " : "│   "), true);
                }
                Print(l.Body, indent + (isLast ? "    " : "│   "), true);
                break;

            case Print p:
                Console.WriteLine("Print");
                Print(p.Expression, indent + (isLast ? "    " : "│   "), true);
                break;

            case ExpressionStmt e:
                Console.WriteLine("ExpressionStmt");
                Print(e.Expression, indent + (isLast ? "    " : "│   "), true);
                break;

            default:
                Console.WriteLine($"(Nodo desconocido tipo {node.GetType().Name})");
                break;
        }
    }
}

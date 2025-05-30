public class Evaluate : IVisitor<object>
{
    private WallEState state = new WallEState();
    private Dictionary<string, object> memory = new();



    public object Visit(GoTo node)
    {
        throw new Exception("No se puede evaluar un 'GoTo' en modo calculadora.");
    }

    public object Visit(Label node)
    {
        throw new Exception("No se puede evaluar una etiqueta.");
    }

    public object Visit(InstructionNode node)
    {
        var args = node.Parameters.Select(e => evaluate(e)).ToList();

        switch (node.InstructionName)
        {
            case "Spawn":
                ExpectTypes(args, typeof(int), typeof(int));
                state.X = Convert.ToInt32(args[0]);
                state.Y = Convert.ToInt32(args[1]);
                break;

            case "Color":
                ExpectTypes(args, typeof(string));
                var color = (string)args[0];

                if (!TokenTypeExtensions.ColorValue.Contains(color))
                    throw new Exception($"Color no válido: '{color}'");

                state.BrushColor = color;
                break;


            case "Size":
                ExpectTypes(args, typeof(int));
                int size = Convert.ToInt32(args[0]);
                if (size % 2 == 0 || size < 1)
                    throw new Exception("El tamaño de la brocha debe ser un número impar mayor o igual que 1.");
                state.BrushSize = size;
                break;

            case "DrawLine":
                ExpectTypes(args, typeof(int), typeof(int), typeof(int));
                int dx = Convert.ToInt32(args[0]);
                int dy = Convert.ToInt32(args[1]);
                int length = Convert.ToInt32(args[2]);
                if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
                    throw new Exception("Dirección inválida. dx y dy deben ser -1, 0 o 1.");
                state.DrawLine(dx, dy, length);
                break;
            case "DrawRectangle":
                ExpectTypes(args, typeof(int), typeof(int), typeof(int), typeof(int), typeof(int));
                int dirx = Convert.ToInt32(args[0]);
                int diry = Convert.ToInt32(args[1]);
                int dist = Convert.ToInt32(args[2]);
                int w = Convert.ToInt32(args[3]);
                int h = Convert.ToInt32(args[4]);
                state.DrawRectangle(dirx, diry, dist, w, h);
                break;

            case "DrawCircle":
                ExpectTypes(args, typeof(int), typeof(int), typeof(double));
                int dix = Convert.ToInt32(args[0]);
                int diy = Convert.ToInt32(args[1]);
                int r = Convert.ToInt32(args[2]);
                state.DrawCircle(dix, diy, r);
                break;

            case "Fill":
                ExpectTypes(args); // no se espera ningún argumento
                state.FillFrom(state.X, state.Y, state.Canvas[state.X, state.Y]);
                break;

            default:
                throw new Exception($"Instrucción desconocida: {node.InstructionName}");
        }

        return null;
    }



    public object Visit(Variable variable)
    {
        if (!memory.TryGetValue(variable.Name, out var value))
            throw new Exception($"Variable '{variable.Name}' no definida");
        return value;
    }

    public object Visit(Assignment assignment)
    {
        var value = evaluate(assignment.Value);
        memory[assignment.NameVariable] = value;
        return value;
    }

    public object Visit(BinaryBoolean binaryBoolean)
    {
        var left = evaluate(binaryBoolean.Left);
        var right = evaluate(binaryBoolean.Right);

        // Lógica para operadores lógicos
        if (binaryBoolean.Op.Type == TokenTypeExtensions.TokenType.And ||
            binaryBoolean.Op.Type == TokenTypeExtensions.TokenType.Or)
        {
            if (left is bool l && right is bool r)
            {
                return binaryBoolean.Op.Type == TokenTypeExtensions.TokenType.And
                    ? l && r
                    : l || r;
            }
            else
            {
                throw new Exception("Operandos de '&&' o '||' deben ser booleanos.");
            }
        }

        // Lógica para operadores relacionales numéricos
        if (left is int lNum && right is int rNum)
        {
            return binaryBoolean.Op.Type switch
            {
                TokenTypeExtensions.TokenType.Equal => lNum == rNum,
                TokenTypeExtensions.TokenType.Greater => lNum > rNum,
                TokenTypeExtensions.TokenType.Less => lNum < rNum,
                TokenTypeExtensions.TokenType.GreaterEqual => lNum >= rNum,
                TokenTypeExtensions.TokenType.LessEqual => lNum <= rNum,
                _ => throw new Exception("Operador booleano inválido.")
            };
        }

        throw new Exception("Operandos incompatibles en operación booleana.");
    }

    public object Visit(BinaryAritmethic binaryAritmethic)
    {
        var left = evaluate(binaryAritmethic.Left);
        var right = evaluate(binaryAritmethic.Right);

        if (left is int l && right is int r)
        {
            return binaryAritmethic.Op.Type switch
            {
                TokenTypeExtensions.TokenType.Plus => l + r,
                TokenTypeExtensions.TokenType.Minus => l - r,
                TokenTypeExtensions.TokenType.Multiply => l * r,
                TokenTypeExtensions.TokenType.Divide => l / r,
                TokenTypeExtensions.TokenType.Pow => Math.Pow(l, r),
                TokenTypeExtensions.TokenType.Modulus => l % r,
                _ => throw new Exception("Operador aritmético inválido")
            };
        }

        throw new Exception("Operandos no numéricos en operación aritmética");
    }
    public object Visit(UnaryExpression unaryExpression)
    {
        var value = evaluate(unaryExpression.Right);

        if (value is int v)
            return -v;

        throw new Exception("Operando no numérico en operación unaria");
    }

    public object Visit(FunctionCallNode node)
    {
        var args = node.Arguments.Select(e => evaluate(e)).ToList();

        switch (node.FunctionName)
        {
            case "GetActualX":
                ExpectTypes(args); // sin argumentos
                return state.X;

            case "GetActualY":
                ExpectTypes(args); // sin argumentos
                return state.Y;

            case "GetCanvasSize":
                ExpectTypes(args); // sin argumentos
                return state.CanvasSize;

            case "GetColorCount":
                ExpectTypes(args, typeof(string), typeof(int), typeof(int), typeof(int), typeof(int));
                string targetColor = Convert.ToString(args[0]);
                int count = 0;
                int x1 = Convert.ToInt32(args[1]);
                int y1 = Convert.ToInt32(args[2]);
                int x2 = Convert.ToInt32(args[3]);
                int y2 = Convert.ToInt32(args[4]);
                for (int i = x1; i <= x2; i++)
                {
                    for (int j = y1; j <= y2; j++)
                    {
                        if (i >= 0 && i < state.CanvasSize && j >= 0 && j < state.CanvasSize)
                        {
                            if (state.Canvas[i, j] == targetColor)
                                count++;
                        }
                    }
                }
                return count;

            case "IsBrushColor":
                ExpectTypes(args, typeof(string));
                return state.BrushColor == (string)args[0];

            case "IsBrushSize":
                ExpectTypes(args, typeof(int));
                return state.BrushSize == (int)args[0];

            case "IsCanvasColor":
                ExpectTypes(args, typeof(string), typeof(int), typeof(int));
                string col = Convert.ToString(args[0]);
                int v = Convert.ToInt32(args[1]);
                int h = Convert.ToInt32(args[2]);

                int x = state.X + h;
                int y = state.Y + v;
                if (!EsValido(x, y)) return false;
                return state.Canvas[x, y] == col;

            default:
                throw new Exception($"Función desconocida: {node.FunctionName}");
        }
    }


    public object? Visit(GroupingExpr groupingExpr)
    {
        return evaluate(groupingExpr.Group);
    }
    public object Visit(Number number)
    {
        return number.Value;
    }

    public object Visit(StringNode stringNode)
    {
        return stringNode.Value;
    }

    public object? evaluate(ASTNode expr)
    {
        return expr.Accept(this);
    }

    private void ExpectTypes(List<object> args, params Type[] expected)
    {
        if (args.Count != expected.Length)
            throw new Exception($"Se esperaban {expected.Length} argumentos.");

        for (int i = 0; i < expected.Length; i++)
        {
            if (args[i] == null || args[i].GetType() != expected[i])
                throw new Exception($"El argumento {i + 1} debe ser de tipo {expected[i].Name}");
        }
    }

    private bool EsValido(int x, int y)
    {
        if (x < 0 || y < 0 || x > state.CanvasSize || y > state.CanvasSize)
        {
            return false;
        }
        return true;
    }
    public IReadOnlyDictionary<string, object> Memory => memory;
    public WallEState State => state;

}
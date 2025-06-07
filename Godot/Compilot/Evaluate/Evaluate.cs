using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class GoToResult
{
    public string Label { get; }
    public GoToResult(string label) => Label = label;
}
public class Evaluate : IVisitor<object>
{
     private WallEState state;

    public Evaluate(WallEState state)
    {
        this.state = state;
    }
    private Dictionary<string, object> memory = new();
    public List<ErrorException> ErroresEvaluacion { get; } = new List<ErrorException>();



   public void EvaluateProgram(List<ASTNode> nodos)
    {
        int i = 0;
        while (i < nodos.Count)
        {
            try
            {
                var node = nodos[i];
                var result = node.Accept(this);

                if (result is GoToResult goToResult)
                {
                    int newPos = nodos.FindIndex(n => n is Label label && label.Value == goToResult.Label);
                    if (newPos == -1)
                    {
                       // ErroresEvaluacion.Add(new ErrorException("Etiqueta '{goToResult.Label}' no encontrada.",));
                        i++;
                        continue;
                    }
                    i = newPos + 1;
                    continue;
                }
            }
            catch (ErrorException e)
            {
                ErroresEvaluacion.Add(e);
            }
            i++;
        }
    }


    public object? Visit(GoTo node)
    {
        var condition = evaluate(node.Condition); // Evalúa la condición
        if (condition is bool b && b) // Si la condición es verdadera
        {
            return new GoToResult(node.LabelName); // Retorna un GoToResult con la etiqueta
        }
        return null; // Si la condición es falsa, no se realiza el salto
    }

    public object? Visit(Label node)
    {
        return null;
    }

    public object? Visit(InstructionNode node)
    {
        var args = node.Parameters.Select(e => evaluate(e)).ToList();

        switch (node.InstructionName)
        {
            case "Spawn":
                ExpectTypes(node,args, typeof(int), typeof(int));
                state.X = Convert.ToInt32(args[0]);
                state.Y = Convert.ToInt32(args[1]);
                break;

            case "Color":
                ExpectTypes(node,args, typeof(string));
               var color = args[0] as string ?? throw new ErrorException("Se esperaba un string", node.OriginToken.Line, node.OriginToken.Position);


                if (!TokenTypeExtensions.ColorValue.Contains(color))
                    throw new ErrorException($"Color no válido: '{color}'",node.OriginToken.Line,node.OriginToken.Position);

                state.BrushColor = color;
                break;


            case "Size":
                ExpectTypes(node,args, typeof(int));
                int size = Convert.ToInt32(args[0]);
                if (size % 2 == 0 || size < 1)
                throw new ErrorException("El tamaño de la brocha debe ser un número impar mayor o igual que 1.",
                node.OriginToken.Line,node.OriginToken.Position);
                state.BrushSize = size;
                break;
            
            case "DrawLine":
                ExpectTypes(node,args, typeof(int), typeof(int), typeof(int));
                int dx = Convert.ToInt32(args[0]);
                int dy = Convert.ToInt32(args[1]);
                int length = Convert.ToInt32(args[2]);
                if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
                throw new ErrorException("Dirección inválida. dx y dy deben ser -1, 0 o 1.",
                node.OriginToken.Line,node.OriginToken.Position);
                state.DrawLine(dx, dy, length);
                break;
            case "DrawRectangle":
                ExpectTypes(node,args, typeof(int), typeof(int), typeof(int), typeof(int), typeof(int));
                int dirx = Convert.ToInt32(args[0]);
                int diry = Convert.ToInt32(args[1]);
                int dist = Convert.ToInt32(args[2]);
                int w = Convert.ToInt32(args[3]);
                int h = Convert.ToInt32(args[4]);
                if (Math.Abs(dirx) > 1 || Math.Abs(diry) > 1)
                throw new ErrorException("Dirección inválida. dx y dy deben ser -1, 0 o 1.",
                node.OriginToken.Line,node.OriginToken.Position);
                state.DrawRectangle(dirx, diry, dist, w, h);
                break;

            case "DrawCircle":
                ExpectTypes(node,args, typeof(int), typeof(int), typeof(int));
                int dix = Convert.ToInt32(args[0]);
                int diy = Convert.ToInt32(args[1]);
                int r = Convert.ToInt32(args[2]);
                if (Math.Abs(dix) > 1 || Math.Abs(diy) > 1)
                throw new ErrorException("Dirección inválida. dx y dy deben ser -1, 0 o 1.",
                node.OriginToken.Line,node.OriginToken.Position);
                state.DrawCircle(dix, diy, r);
                break;

            case "Fill":
                ExpectTypes(node,args); // no se espera ningún argumento
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
            throw new ErrorException($"Variable '{variable.Name}' no definida", variable.OriginToken.Line, variable.OriginToken.Position);
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
    // 1) Evaluar recursivamente ambos operandos
    object? left  = evaluate(binaryBoolean.Left);
    object? right = evaluate(binaryBoolean.Right);

    // 2) Si el operador es AND (&&) u OR (||), ambos deben ser bool
    if (binaryBoolean.Op.Type == TokenTypeExtensions.TokenType.And ||
        binaryBoolean.Op.Type == TokenTypeExtensions.TokenType.Or)
    {
        if (!(left  is bool) || !(right is bool))
        {
            throw new ErrorException(
                "Operandos de '&&' o '||' deben ser booleanos.",
                binaryBoolean.OriginToken.Line,
                binaryBoolean.OriginToken.Position
            );
        }
        return binaryBoolean.Op.Type == TokenTypeExtensions.TokenType.And
            ? (bool)left && (bool)right
            : (bool)left || (bool)right;
    }

    // 3) De lo contrario, deben ser comparaciones numéricas (==, <, >, <=, >=):
    if (!(left  is int) || !(right is int))
    {
        throw new ErrorException(
            "Operandos de comparación deben ser enteros.",
            binaryBoolean.OriginToken.Line,
            binaryBoolean.OriginToken.Position
        );
    }

    int lNum = (int)left;
    int rNum = (int)right;

    return binaryBoolean.Op.Type switch
    {
        TokenTypeExtensions.TokenType.Equal        => lNum == rNum,
        TokenTypeExtensions.TokenType.Greater      => lNum > rNum,
        TokenTypeExtensions.TokenType.Less         => lNum < rNum,
        TokenTypeExtensions.TokenType.GreaterEqual => lNum >= rNum,
        TokenTypeExtensions.TokenType.LessEqual    => lNum <= rNum,
        _ => throw new ErrorException(
                 "Operador booleano inválido.",
                 binaryBoolean.OriginToken.Line,
                 binaryBoolean.OriginToken.Position
             )
    };
}


    public object Visit(BinaryAritmethic binaryAritmethic)
{
    // 1) Evaluamos recursivamente ambos operandos
    object? left  = evaluate(binaryAritmethic.Left);
    object? right = evaluate(binaryAritmethic.Right);

    // 2) Chequeamos que AMBOS sean int
    if (!(left is int) || !(right is int))
    {
        throw new ErrorException(
            $"El operando debe ser de tipo Int.",
            binaryAritmethic.OriginToken.Line,
            binaryAritmethic.OriginToken.Position
        );
    }

    // 3) Como ya sabemos que left/right son int, hacemos el casting
    int l = (int)left;
    int r = (int)right;

    // 4) Ejecutamos la operación aritmética
    return binaryAritmethic.Op.Type switch
    {
        TokenTypeExtensions.TokenType.Plus      => l + r,
        TokenTypeExtensions.TokenType.Minus     => l - r,
        TokenTypeExtensions.TokenType.Multiply  => l * r,
        TokenTypeExtensions.TokenType.Divide    => l / r,
        TokenTypeExtensions.TokenType.Pow       => (int)Math.Pow(l, r),
        TokenTypeExtensions.TokenType.Modulus   => l % r,
        _ => throw new ErrorException(
                 "Operador aritmético inválido.",
                 binaryAritmethic.OriginToken.Line,
                 binaryAritmethic.OriginToken.Position
             )
    };
}

    public object Visit(UnaryExpression unaryExpression)
    {
        var value = evaluate(unaryExpression.Right);

        if (value is int v)
            return -v;

        throw new ErrorException("Operando no numérico en operación unaria",unaryExpression.OriginToken.Line, unaryExpression.OriginToken.Position);
    }

    public object Visit(FunctionCallNode node)
    {
        var args = node.Arguments.Select(e => evaluate(e)).ToList();

        switch (node.FunctionName)
        {
            case "GetActualX":
                ExpectTypes(node,args); // sin argumentos
                return state.X;

            case "GetActualY":
                ExpectTypes(node,args); // sin argumentos
                return state.Y;

            case "GetCanvasSize":
                ExpectTypes(node,args); // sin argumentos
                return state.CanvasSize;

            case "GetColorCount":
                ExpectTypes(node,args, typeof(string), typeof(int), typeof(int), typeof(int), typeof(int));
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
                ExpectTypes(node,args, typeof(string));
                if (state.BrushColor == (string)args[0]) return 1;
                return 0;

            case "IsBrushSize":
                ExpectTypes(node,args, typeof(int));
                if (state.BrushSize == (int)args[0]) return 1;
                return 0;

            case "IsCanvasColor":
                ExpectTypes(node,args, typeof(string), typeof(int), typeof(int));
                string col = Convert.ToString(args[0]);
                int v = Convert.ToInt32(args[1]);
                int h = Convert.ToInt32(args[2]);

                int x = state.X + h;
                int y = state.Y + v;
                if (!EsValido(x, y)) return false;
                if (state.Canvas[x, y] == col) return 1;
                return 0;

            default:
                throw new ErrorException($"Función desconocida: {node.FunctionName}",node.OriginToken.Line,node.OriginToken.Position);
        }
    }


    public object Visit(GroupingExpr groupingExpr)
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

    public object evaluate(ASTNode expr)
{
    var result = expr.Accept(this);
    if (result == null)
    {
        throw new ErrorException(
            "Error: la expresión no devolvió ningún valor.",
            expr.OriginToken.Line,
            expr.OriginToken.Position
        );
    }
    return result;
}


    private void ExpectTypes(ASTNode nodo, List<object?> args, params Type[] expected)
{
    if (args.Count != expected.Length)
        throw new ErrorException(
            $"Se esperaban {expected.Length} argumentos.",
            nodo.OriginToken.Line,
            nodo.OriginToken.Position
        );

    for (int i = 0; i < expected.Length; i++)
    {
        if (args[i] == null || !expected[i].IsInstanceOfType(args[i]))
            throw new ErrorException(
                $"El argumento {i + 1} debe ser de tipo {expected[i].Name}.",
                nodo.OriginToken.Line,
                nodo.OriginToken.Position
            );
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

     private void CheckTypeOperand(ASTNode node,Type expectedType,params object?[] operands)
    {
        foreach (var item in operands)
        {
            if (item == null || !expectedType.IsInstanceOfType(item))
            {
                throw new ErrorException(
                    $"El operando debe ser de tipo {expectedType.Name}.",
                    node.OriginToken.Line,
                    node.OriginToken.Position
                );
            }
        }
    }
    public IReadOnlyDictionary<string, object> Memory => memory;
    public WallEState State => state;

}
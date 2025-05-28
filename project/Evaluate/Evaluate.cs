public class Evaluate : IVisitor<object>
{
    private Dictionary<string, object> memory = new();

    public object Visit(FunctionCallNode node)
{
    throw new Exception("No se puede evaluar una llamada a función en modo calculadora.");
}

public object Visit(GoTo node)
{
    throw new Exception("No se puede evaluar un 'GoTo' en modo calculadora.");
}

public object Visit(InstructionNode node)
{
    throw new Exception("No se puede evaluar una instrucción como expresión.");
}

public object Visit(Label node)
{
    throw new Exception("No se puede evaluar una etiqueta.");
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

        if (left is double l && right is double r)
        {
            return binaryBoolean.Op.Type switch
            {
                TokenTypeExtensions.TokenType.Equal => l == r,
                TokenTypeExtensions.TokenType.Less => l < r,
                TokenTypeExtensions.TokenType.LessEqual => l <= r,
                TokenTypeExtensions.TokenType.Greater => l > r,
                TokenTypeExtensions.TokenType.GreaterEqual => l >= r,
                _ => throw new Exception("Operador booleano inválido")
            };
        }

        throw new Exception("Operandos no numéricos en operación booleana");
    }
    public object Visit(BinaryAritmethic binaryAritmethic)
    {
        var left = evaluate(binaryAritmethic.Left);
        var right = evaluate(binaryAritmethic.Right);

        if (left is double l && right is double r)
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

        if (value is double v)
            return -v;

        throw new Exception("Operando no numérico en operación unaria");
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

    public IReadOnlyDictionary<string, object> Memory => memory;

}
public class Evaluate : IVisitor<object>
{
    private Dictionary<string, object> memory = new();

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
        object left = evaluate(binaryBoolean.Left);
        object right = evaluate(binaryBoolean.Right);

        switch (binaryBoolean.Op.Type)
        {
            case TokenTypeExtensions.TokenType.Equal:
                return (double)left == (double)right;

            case TokenTypeExtensions.TokenType.Less:
                return (double)left < (double)right;

            case TokenTypeExtensions.TokenType.LessEqual:
                return (double)left <= (double)right;

            case TokenTypeExtensions.TokenType.Greater:
                return (double)left > (double)right;

            case TokenTypeExtensions.TokenType.GreaterEqual:
                return (double)left >= (double)right;

            case TokenTypeExtensions.TokenType.Modulus:
                return (double)left % (double)right;
        }
        return null;
    }

    public object Visit(BinaryAritmethic binaryAritmethic)
    {
        object left = evaluate(binaryAritmethic.Left);
        object right = evaluate(binaryAritmethic.Right);

        if (left is double l && right is double r)
        {
            switch (binaryAritmethic.Op.Type)
            {
                case TokenTypeExtensions.TokenType.Plus:
                    return l + r;

                case TokenTypeExtensions.TokenType.Minus:
                    return l - r;

                case TokenTypeExtensions.TokenType.Multiply:
                    return l * r;

                case TokenTypeExtensions.TokenType.Divide:
                    return l / r;

                case TokenTypeExtensions.TokenType.Pow:
                    return Math.Pow(l, r);

                case TokenTypeExtensions.TokenType.Modulus:
                    return l % r;
            }
            return null;
        }
        else
        {
            throw new Exception("Operación inválida: operandos no numéricos");
        }

    }
    public object Visit(UnaryExpression unaryExpression)
    {
        object? right = evaluate(unaryExpression.Right);
        return -(double)right;
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

}
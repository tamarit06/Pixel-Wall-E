public class UnaryExpression: Expression
{
    public Token Operator{get;}
    public Expression Right{get;}

    public UnaryExpression(Token op, Expression right )
    {
        Operator=op;
        Right=right;
    }
}
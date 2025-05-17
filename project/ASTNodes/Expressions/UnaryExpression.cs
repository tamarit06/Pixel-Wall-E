public class UnaryExpression:ASTNode
{
    public Token Operator{get;}
    public ASTNode Right{get;}

    public UnaryExpression(Token op, ASTNode right )
    {
        Operator=op;
        Right=right;
    }
}
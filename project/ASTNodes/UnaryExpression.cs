public class UnaryExpression : ASTNode
{
    public Token Operator { get; }
    public ASTNode Right { get; }

    public UnaryExpression(Token op, ASTNode right):base(op)
    {
        Operator = op;
        Right = right;
    }
    
     public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
public class BinaryBoolean : BinaryExpression
{
    public BinaryBoolean(ASTNode left, Token op, ASTNode right) : base(left, op, right) { }
     public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
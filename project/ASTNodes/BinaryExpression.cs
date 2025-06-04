public abstract class BinaryExpression: ASTNode
{
    public ASTNode Left{get;}
    public Token Op{get;}
    public ASTNode Right{get;}

    public BinaryExpression(ASTNode left, Token op, ASTNode right):base(op)
    {
        Left=left;
        Op=op;
        Right=right;
    }
}
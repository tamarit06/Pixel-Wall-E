public abstract class OpBinaria:ASTNode
{
    public ASTNode Left{get; private set; }
    public Token Op{get; private set;}
    public ASTNode Right{get; private set;}

    public OpBinaria(ASTNode left, Token Op, ASTNode right)
    {
        Left=left;
        Op=Op;
        Right=right;
    }

}
public abstract class ASTNode
{
    public Token OriginToken { get; }

    protected ASTNode(Token origin)
    {
        OriginToken = origin;
    }

    public abstract T Accept<T>(IVisitor<T> visitor);
}

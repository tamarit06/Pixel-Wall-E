public class GroupingExpr : ASTNode
{
    public ASTNode Group { get; }

    public GroupingExpr(Token originToken, ASTNode group)
        : base(originToken)  // Guarda el token en la clase base ASTNode
    {
        Group = group;
    }

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}

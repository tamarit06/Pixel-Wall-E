public class GroupingExpr: ASTNode
{
    public ASTNode Group { get; private set; }

    public GroupingExpr(ASTNode group)
    {
        Group = group;
    }

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}
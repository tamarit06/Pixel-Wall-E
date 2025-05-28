public class GoTo : ASTNode
{
    public string Label { get; }
    public ASTNode Condition { get; }

    public GoTo(string label, ASTNode condition)
    {
        Label = label;
        Condition = condition;
    }
     public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

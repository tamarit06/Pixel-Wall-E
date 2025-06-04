public class GoTo : ASTNode
{
    public string LabelName { get; }
    public ASTNode Condition { get; }

    public GoTo(Token originToken, string labelName, ASTNode condition)
        : base(originToken)
    {
        LabelName = labelName;
        Condition = condition;
    }

    public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
}

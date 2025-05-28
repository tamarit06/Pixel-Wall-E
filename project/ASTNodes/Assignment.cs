public class Assignment : ASTNode
{
    public string NameVariable { get; }
    public ASTNode Value { get; }

    public Assignment(string nameVal, ASTNode value)
    {
        NameVariable = nameVal;
        Value = value;
    }
    
     public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

}
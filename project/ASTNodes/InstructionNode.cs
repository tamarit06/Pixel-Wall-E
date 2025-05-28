public class InstructionNode : ASTNode
{
    public string InstructionName { get; }
    public List<ASTNode> Parameters { get; }

    public InstructionNode(string name, List<ASTNode> parameters)
    {
        InstructionName = name;
        Parameters = parameters;
    }
     public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

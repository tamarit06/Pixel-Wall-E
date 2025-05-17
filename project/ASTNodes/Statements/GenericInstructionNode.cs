public class GenericInstructionNode : ASTNode
{
    public string InstructionName { get; }
    public List<ASTNode> Parameters { get; }

    public GenericInstructionNode(string name, List<ASTNode> parameters)
    {
        InstructionName = name;
        Parameters = parameters;
    }
}

public class FunctionCallNode : ASTNode
{
    public string FunctionName { get; }
    public List<ASTNode> Arguments { get; }

    public FunctionCallNode(string name, List<ASTNode> arguments)
    {
        FunctionName = name;
        Arguments = arguments;
    }
}

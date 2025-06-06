using System.Collections.Generic;
public class FunctionCallNode : ASTNode
{
    public string FunctionName { get; }
    public List<ASTNode> Arguments { get; }

    public FunctionCallNode(Token token, List<ASTNode> arguments) : base(token)
    {
        FunctionName = token.Lexeme;
        Arguments = arguments;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

using System;
using System.Collections.Generic;
public class InstructionNode : ASTNode
{
    public string InstructionName { get; }
    public List<ASTNode> Parameters { get; }

    public InstructionNode(Token instToken, List<ASTNode> parameters) : base(instToken)
    {
        InstructionName = instToken.Lexeme;
        Parameters = parameters;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

using System.Collections;
using System.Collections.Generic;
public class Variable : ASTNode
{
    public string Name { get; }

    public Variable(Token token)
    {
        Name = token.Lexeme;
    }
  public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
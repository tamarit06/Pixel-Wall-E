public class Assignment : ASTNode
{
    public string NameVariable { get; }
    public ASTNode Value { get; }

    public Assignment(Token token, ASTNode value):base(token)
    {
        NameVariable = token.Lexeme;
        Value = value;
    }
    
     public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

}
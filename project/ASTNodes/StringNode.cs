public class StringNode : ASTNode
{
    public Token String_{ get; }
    public string Value { get; }

    public StringNode(Token string_):base(string_)
    {
        String_= string_;
        Value = string_.Lexeme;
    }
    
     public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
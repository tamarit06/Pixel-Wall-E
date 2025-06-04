public class Number : ASTNode
{
    public int Value { get; private set; }

    public Number(Token num) : base(num)
    {
        Value = int.Parse(num.Lexeme);
    }

     public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
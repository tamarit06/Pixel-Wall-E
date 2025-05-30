public class Number : ASTNode
{
    public Token Num { get; private set; }
    public int Value { get; private set; }

    public Number(Token num)
    {
        Num = num;
        Value = int.Parse(num.Lexeme);
    }

     public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
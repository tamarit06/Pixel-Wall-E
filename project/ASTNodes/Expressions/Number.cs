public class Number : ASTNode
{
    public Token Num { get; private set; }
    public double Value { get; private set; }

    public Number(Token num)
    {
        Num = num;
        Value = double.Parse(num.Lexeme);
    }
}
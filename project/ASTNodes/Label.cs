public class Label : ASTNode
{
    public string Value { get; }
    public Label(string value) => Value = value;
    public override T Accept<T>(IVisitor<T> visitor)
    {
      return visitor.Visit(this);
   }
}

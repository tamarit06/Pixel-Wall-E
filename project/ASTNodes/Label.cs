public class Label : ASTNode
{
    public string Value { get; }
   public Label(Token labelToken) : base(labelToken)
   {
      Value = labelToken.Lexeme;
    }
    public override T Accept<T>(IVisitor<T> visitor)
   {
      return visitor.Visit(this);
   }
}

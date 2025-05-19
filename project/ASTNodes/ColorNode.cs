public class ColorNode:ASTNode
{
    public Token Color{get;}
    public string Value{get;}

    public ColorNode(Token color_)
    {
        Color=color_;
        Value=color_.Lexeme;
    }
}
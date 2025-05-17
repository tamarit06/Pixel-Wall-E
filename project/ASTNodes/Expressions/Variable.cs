public class Variable:ASTNode
{
    public string Name{get;}

    public Variable(Token token)
    {
        Name=token.Lexeme;
    } 
}
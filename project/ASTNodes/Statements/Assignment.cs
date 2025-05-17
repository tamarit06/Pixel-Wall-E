public class Assignment: Statement
{
    public string NameVariable{get;}
    public ASTNode Value{get;}

    public Assignment(string nameVal,ASTNode value)
    {
        NameVariable=nameVal;
        Value=value;
    }

}
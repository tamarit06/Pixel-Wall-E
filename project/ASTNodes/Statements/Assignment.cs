public class Assignment: Statement
{
    public string NameVariable{get;}
    public Expression Value{get;}

    public Assignment(string nameVal,Expression value)
    {
        NameVariable=nameVal;
        Value=value;
    }

}
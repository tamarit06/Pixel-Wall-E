using System.Collections.Generic;
using System.Collections;

public class InstructionStatement: Statement
{
    public string Name {get;}
    public List<Expression> Parameters{get;}

    public InstructionStatement(string name, List<Expression> parameters)
    {
        Name=name;
        Parameters= parameters;
    }
}
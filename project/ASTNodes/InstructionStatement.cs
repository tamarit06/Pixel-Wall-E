using System.Collections.Generic;
using System.Collections;

public class InstructionStatement: Statement
{
    public string Name {get;}
    public List<ASTNode> Parameters{get;}

    public InstructionStatement(string name, List<ASTNode> parameters)
    {
        Name=name;
        Parameters= parameters;
    }
}
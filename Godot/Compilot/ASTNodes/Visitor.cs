using System.Collections;
using System.Collections.Generic;
public interface IVisitor<T>
{
    T Visit(Number number);
   T Visit(Assignment assignment);
    T Visit(BinaryAritmethic binaryAritmethic);
   T Visit(BinaryBoolean binaryBoolean);
   T Visit(FunctionCallNode functionCallNode);
    T Visit(GoTo goTo);
    T Visit(GroupingExpr groupingExpr);
   T Visit(InstructionNode instructionNode);
    T Visit(Label label);
    T Visit(StringNode stringNode);
    T Visit(UnaryExpression unaryExpression);
   T Visit(Variable  variable);
}
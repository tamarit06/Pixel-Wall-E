using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static TokenTypeExtensions;

public class Parser
{
    public Lexer lexer{get; private set;}
    private int position;

    public List<ASTNode> Nodos {get; private set;}

    public Parser(Lexer lexer)
    {
        this.lexer = lexer;
        position=0;
        Nodos=new List<ASTNode>();
    }

   private bool spawnUsado = false;

public void Parsind()
{
    if (!IsAtEnd)
    {
        if (Current.Lexeme != "Spawn")
            throw new ParserException($"El programa debe comenzar con Spawn (línea {Current.Linea})");

        Nodos.Add(ParseGenericInstruction());
        spawnUsado = true;
    }

    while (!IsAtEnd)
    {
        var stmt = ParseStatement();
        if (stmt != null)
            Nodos.Add(stmt);
    }
}

private ASTNode ParseStatement()
{
    // si es un identificador y luego abre parentesis
    if (Current.Type == TokenType.Identifier && NextIs(TokenType.OpenParen))
    {
         return ParseGenericInstruction();
       /* switch (Current.Lexeme)
        {
            case "Spawn":
                if (spawnUsado)
                    throw new ParserException($"Solo se permite un Spawn (línea {Current.Linea})");
                spawnUsado = true;
                return Spawn();

            case "Color":
                return ParseColor();
            
            case "Size":
            return Size();

            case "DrawLine":
            return DrawLine();

            case "DrawCircle":
            return DrawCircle();

            case "DrawRectangle":
            return DrawRectangle();

            case "Fill":
            return Fill();
        }*/
    }



    if (Current.Type == TokenType.GoTo)
    {
        return ParseGoTo();
    }
    // Si es un identificador y no es asignación ni instrucción, puede ser una etiqueta
    if (Current.Type == TokenType.Identifier && !NextIs(TokenType.AssigmentExpresions))
    {
        return LabelParse();
    }


     if (NextIs(TokenType.AssigmentExpresions))
     {
        return ParseAssignment();
     }

}

private ASTNode ParseGenericInstruction()
{
    var name = Consume(TokenType.Identifier, "Se esperaba nombre de instrucción").Lexeme;

    Consume(TokenType.OpenParen, "Se esperaba '('");

    List<ASTNode> parameters = new();

    if (Current.Type != TokenType.CloseParen)
    {
        do
        {
            parameters.Add(Expression()); // puede ser número, color, variable, función, etc.
        } while (Match(TokenType.Comma) && Advance() != null);
    }

    Consume(TokenType.CloseParen, "Se esperaba ')'");

    return new GenericInstructionNode(name, parameters);
}

private ASTNode LabelParse()
{
    var labelToken = Consume(TokenType.Identifier, "Se esperaba nombre de etiqueta");
    return new Label(labelToken.Lexeme);
}

private ASTNode ParseGoTo()//ver esto
{
    Consume(TokenType.GoTo, "Se esperaba 'GoTo'");

    Consume(TokenType.OpenBracket, "Se esperaba '['");
    var labelName = Consume(TokenType.Identifier, "Se esperaba el nombre de una etiqueta").Lexeme;
    Consume(TokenType.CloseBracket, "Se esperaba ']'");

    Consume(TokenType.OpenParen, "Se esperaba '('");

    var condition = BooleanExpression();

    Consume(TokenType.CloseParen, "Se esperaba ')'");

    return new Goto(labelName, condition);
}


private ASTNode ParseAssignment()
{
    var variable = Consume(TokenType.Identifier, "Se esperaba un nombre de variable");

    Consume(TokenType.AssigmentExpresions, "Se esperaba '<-' para la asignación");

    var expression = Expression();

    return new Assignment(variable.Lexeme, expression);
}

// 1. Máximo nivel (lógico OR)
private ASTNode Expression()
{
    var left = And();
    while (Match(TokenType.Or))
    {
        var op = Advance();
        var right = And();
        left = new BinaryExpression(left, op, right);
    }
    return left;
}

// 2. Operador lógico AND
private ASTNode And()
{
    var left = Comparison();
    while (Match(TokenType.And))
    {
        var op = Advance();
        var right = Comparison();
        left= new BinaryExpression(left, op, right);
    }
    return left;
}

// 3. Comparaciones (==, <, >, etc.)
private ASTNode Comparison()
{
    var left = Term();
    while (Match(TokenType.Equal, TokenType.Greater, TokenType.Less, 
                 TokenType.GreaterEqual, TokenType.LessEqual))
    {
        var op = Advance();
        var right = Term();
        left = new BinaryExpression(left, op, right);
    }
    return left;
}

// 4. Suma y resta
private ASTNode Term()
{
    ASTNode left = Factor();
    while (Match(TokenType.Plus, TokenType.Minus))
    {
        var op = Advance();
        var right = Factor();
        left = new BinaryExpression(left, op, right);
    }
    return left;
}

// 5. Multiplicación, división y módulo
private ASTNode Factor()
{
    ASTNode left = Unary();
    while (Match(TokenType.Multiply, TokenType.Divide, TokenType.Modulus))
    {
        var op = Advance();
        var right = Unary();
        left = new BinaryExpression(left, op, right);
    }
    return left;
}

//6 Potencia
private ASTNode Pow()
{
    ASTNode left= Unary();
    if (Match(TokenType.Pow))
    {
        Token op=Advance();
        ASTNode right=Pow();
        left=new BinaryExpression(left, op, right);
    }
    return left;
}

// 7. Unarios
private ASTNode Unary()
{
    if (Match(TokenType.Minus))
    {
        var op = Advance();
        var operand = Unary();
        return new UnaryExpression(op, operand);
    }
    return Primary();
}

//8 Funciones
private ASTNode FunctionCall()
{
    var name = Consume(TokenType.Identifier, "Se esperaba nombre de función").Lexeme;

    Consume(TokenType.OpenParen, "Se esperaba '('");

    List<ASTNode> args = new();

    if (Current.Type != TokenType.CloseParen)
    {
        do
        {
            args.Add(Expression());
        } while (Match(TokenType.Comma) && Advance() != null);
    }

    Consume(TokenType.CloseParen, "Se esperaba ')'");

    return new FunctionCallNode(name, args);
}


// 9. Literales, variables, paréntesis
private ASTNode Primary()
{
    if (Match(TokenType.Number))
        return new Number(Advance());

    if (Match(TokenType.Color))
        return new ColorNode(Advance());

    if (Match(TokenType.Identifier) && NextIs(TokenType.OpenParen))
        return FunctionCall();

    if (Match(TokenType.Identifier))
        return new Variable(Advance());

    if (Match(TokenType.OpenParen))
    {
        Advance(); // consume '('
        var expr = Expression();
        Consume(TokenType.CloseParen, "Esperaba ')'");
        return expr;
    }

    throw new ParserException($"Expresión inesperada: {Current.Lexeme}");
}

    private bool IsAtEnd => position >= lexer.Tokens.Count - 1;
    private Token Advance() => lexer.Tokens[position++];
    private Token Current => lexer.Tokens[position];

    private bool NextIs(TokenType tipo)
{
    return position + 1 < lexer.Tokens.Count && lexer.Tokens[position + 1].Type == tipo;
}


     private bool Match(params TokenType[] tokenTypes)
    {
        foreach (TokenType type in tokenTypes)
        {
            if (type == Current.Type) return true;
        }
        return false;
    }

  private Token Consume(TokenType type, string message)
    {
        if (Current.Type== type) return Advance();
        throw new ParserException(message + $" (en token '{Current.Lexeme}')");
    }   


}

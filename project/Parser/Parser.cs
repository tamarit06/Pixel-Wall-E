using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static TokenTypeExtensions;

public class Parser
{
    public Lexer lexer {get;  }
    private int position;

    public List<ASTNode> Nodos { get; private set; }

    public Parser(Lexer lexer)
    {
        this.lexer = lexer;
        position = 0;
        Nodos = new List<ASTNode>();
    }

    private bool spawnUsado = false;

    public ASTNode Parsind()
    {
        if (Check(TokenType.Identifier) && Peek().Lexeme == "Spawn")
            {
                 // Método para parsear la instrucción Spawn
                var spawnNode = ParseGenericInstruction();
                Nodos.Add(spawnNode);
                spawnUsado = true; // Marcar que Spawn ha sido usado
            }
            else
            {
                throw new ParserException("Se esperaba la instrucción 'Spawn(int x,int y)' al inicio del código.",Peek().Linea,Peek().Posicion);
            }
        while (!IsAtEnd)
        {
            // Verificar si el primer nodo es Spawn

            if (Check(TokenType.EOL))
            {
                Advance(); // Consumir el EOL
                continue; // Saltar a la siguiente iteración
            }
            var stmt = ParseStatement(); //Cambia esto si necesitas otro método para parsear
            if (stmt != null)
                Nodos.Add(stmt);
        }

        // Si no hay nodos, puedes lanzar una excepción o devolver un nodo vacío
        if (Nodos.Count == 0)
        {
            throw new ParserException("No se encontraron nodos en la entrada.",Peek().Linea,Peek().Posicion);
        }

        return Nodos[0]; // Retorna el primer nodo si existe
    }


    private ASTNode ParseStatement()
    {
        if (IsAtEnd) return null;
       
        // si es un identificador y luego abre parentesis
        if (Peek().Type == TokenType.Identifier && NextIs(TokenType.OpenParen))
        {
            return ParseGenericInstruction();

        }

        if (Peek().Type == TokenType.GoTo)
        {
            return ParseGoTo();
        }
       
        // Si es un identificador y no es asignación ni instrucción, puede ser una etiqueta
        if (Peek().Type == TokenType.Identifier && !NextIs(TokenType.AssigmentExpresions))
        {
            return LabelParse();
        }

        if (Peek().Type == TokenType.Identifier && NextIs(TokenType.AssigmentExpresions))
        {
            return AssignmentParse();
        }

        // Manejo de otras instrucciones
        throw new ParserException($"Instrucción no reconocida: {Peek().Lexeme}",Peek().Linea,Peek().Posicion);
    }

    private ASTNode ParseGenericInstruction()
    {
        if ( Peek().Lexeme == "Spawn")
    {
        // Si ya se usó Spawn, error
        if (spawnUsado)
        {
            throw new ParserException("La instrucción 'Spawn' solo puede aparecer una vez.",Peek().Linea,Peek().Posicion);
        }
    }
        var name = Consume(TokenType.Identifier, "Se esperaba nombre de instrucción").Lexeme;

        Consume(TokenType.OpenParen, "Se esperaba '('");

        List<ASTNode> parameters = new();

        if (Peek().Type != TokenType.CloseParen)
        {
            do
            {
                parameters.Add(Term()); // puede ser número, color, variable, función, etc.
            } while (Match(TokenType.Comma) && Advance() != null);
        }

        Consume(TokenType.CloseParen, "Se esperaba ')'");
        ConsumeEOL();
        return new GenericInstructionNode(name, parameters);
    }

    private ASTNode ParseGoTo()//ver esto
    {
        Consume(TokenType.GoTo, "Se esperaba 'GoTo'");

        Consume(TokenType.OpenBracket, "Se esperaba '['");
        var labelName = Consume(TokenType.Identifier, "Se esperaba el nombre de una etiqueta").Lexeme;
        Consume(TokenType.CloseBracket, "Se esperaba ']'");

        Consume(TokenType.OpenParen, "Se esperaba '('");

        var condition = Or();

        Consume(TokenType.CloseParen, "Se esperaba ')'");
        ConsumeEOL();
        return new GoTo(labelName, condition);
    }

        private ASTNode LabelParse()
    {
        var labelToken = Consume(TokenType.Identifier, "Se esperaba nombre de etiqueta");
        ConsumeEOL();
        return new Label(labelToken.Lexeme);
    }

    private ASTNode AssignmentParse()
    {
        var variable = Consume(TokenType.Identifier, "Se esperaba un nombre de variable");

        Consume(TokenType.AssigmentExpresions, "Se esperaba '<-' para la asignación");

        var expression = Or();
        ConsumeEOL();

        return new Assignment(variable.Lexeme, expression);
    }

    // 1. Máximo nivel (lógico OR)
    private ASTNode Or()
    {
        ASTNode left = And();
        while (Match(TokenType.Or))
        {
            var op = Advance();
            ASTNode right = And();
            left = new BinaryExpression(left, op, right);
        }
        return left;
    }

    // 2. Operador lógico AND
    private ASTNode And()
    {
        ASTNode left = Equality();
        while (Match(TokenType.And))
        {
            var op = Advance();
            ASTNode right = Equality();
            left = new BinaryExpression(left, op, right);
        }
        return left;
    }

    //3. Igualdad(==)
    private ASTNode Equality()
    {
        ASTNode left = Comparison();
        while (Match(TokenType.Equal))
        {
            Token op = Advance();
            ASTNode right = Comparison();
            left = new BinaryExpression(left, op, right);
        }
        return left;
    }
    // 4. Comparaciones (<, >, <=,>=)
    private ASTNode Comparison()
    {
        ASTNode left = Term();
        while (Match(TokenType.Greater, TokenType.Less,
                     TokenType.GreaterEqual, TokenType.LessEqual))
        {
            var op = Advance();
            ASTNode right = Term();
            left = new BinaryExpression(left, op, right);
        }
        return left;
    }

    // 5. Suma y resta
    private ASTNode Term()
    {
        ASTNode left = Factor();
        while (Match(TokenType.Plus, TokenType.Minus))
        {
            Token op = Advance();
            ASTNode right = Factor();
            left = new BinaryExpression(left, op, right);
        }
        return left;
    }

    // 6. Multiplicación, división y módulo
    private ASTNode Factor()
    {
        ASTNode left = Pow();
        while (Match(TokenType.Multiply, TokenType.Divide, TokenType.Modulus))
        {
            var op = Advance();
            var right = Pow();
            left = new BinaryExpression(left, op, right);
        }
        return left;
    }

    //7 Potencia
    private ASTNode Pow()
    {
        ASTNode left = Unary();
        if (Match(TokenType.Pow))
        {
            Token op = Advance();
            ASTNode right = Pow();
            left = new BinaryExpression(left, op, right);
        }
        return left;
    }

    // 8. Unarios
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

    //9 Funciones
    private ASTNode FunctionCall()
    {
        var name = Consume(TokenType.Identifier, "Se esperaba nombre de función").Lexeme;

        Consume(TokenType.OpenParen, "Se esperaba '('");

        List<ASTNode> args = new();

        if (Peek().Type != TokenType.CloseParen)
        {
            do
            {
                args.Add(Or());
            } while (Match(TokenType.Comma) && Advance() != null);
        }

        Consume(TokenType.CloseParen, "Se esperaba ')'");

        return new FunctionCallNode(name, args);
    }


    // 10. Literales, variables, paréntesis
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
            var expr = Or();//cambiar
            Consume(TokenType.CloseParen, "Esperaba ')'");
            return expr;
        }

        throw new ParserException($"Expresión inesperada: {Peek().Lexeme}",Peek().Linea,Peek().Posicion);
    }

    private bool IsAtEnd => position >= lexer.Tokens.Count;

    private Token Peek()
    {
        if (IsAtEnd)
            throw new ParserException("No hay más tokens para procesar.",Peek().Linea,Peek().Posicion);
        return lexer.Tokens[position]; // Devuelve el token actual sin avanzar
    }

    private Token Previous()
    {
        if (position == 0)
            throw new ParserException("No hay token anterior.",Peek().Linea,Peek().Posicion);
        return lexer.Tokens[position - 1]; // Devuelve el token anterior
    }

    private bool Check(TokenType type)
    {
        if (IsAtEnd) return false; // Si no hay más tokens, devuelve false
        return Peek().Type == type; // Compara el tipo del token actual con el tipo esperado
    }
    private bool NextIs(TokenType type)
    {
        if (position + 1 >= lexer.Tokens.Count) return false; // Verifica si hay un siguiente token
        return lexer.Tokens[position + 1].Type == type; // Compara el tipo del siguiente token con el tipo esperado
    }

    private Token Advance()
    {
        if (!IsAtEnd)
        {
            return lexer.Tokens[position++]; // Devuelve el token actual y avanza la posición
        }
        throw new ParserException("No hay más tokens para procesar.",Peek().Linea,Peek().Posicion);
    }


    private bool Match(params TokenType[] tokenTypes)
    {
        foreach (TokenType type in tokenTypes)
        {
            if (Check(type)) // Usa Check para verificar el tipo
                return true;
        }
        return false;
    }

    private Token Consume(TokenType type, string message)
    {
        if (Check(type)) // Cambia Current por Check
        {
            return Advance(); // Consume el token y avanza
        }

        // Solo llama a Previous si hay un token anterior
        string previousLexeme = position > 0 ? Previous().Lexeme : "N/A";
        throw new ParserException($"{message} (en token '{previousLexeme}')", Peek().Linea,Peek().Posicion);
    }

    private void ConsumeEOL()
    {
        if (Check(TokenType.EOL) || Check(TokenType.EOF))
        {
            Advance(); // Consumir el EOL
        }
        else
        {
            throw new ParserException("Se esperaba un salto de línea",Peek().Linea,Peek().Posicion);
        }
    }


}

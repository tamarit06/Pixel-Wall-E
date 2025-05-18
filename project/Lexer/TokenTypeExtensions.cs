using System.Collections.Generic;
using System.Collections;
public class TokenTypeExtensions
{
    public static readonly Dictionary<TokenType, string> TokenGeneral;
    
    static TokenTypeExtensions()
    {
         TokenGeneral = new()
        {
            {TokenType.Number, @"^\b\d+\b" },
            {TokenType.Identifier,@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ][a-zA-ZñÑáéíóúÁÉÍÓÚ0-9_-]*" },
            {TokenType.Color,"^\"[^\"]*\""},
            {TokenType.AssigmentExpresions, @"^\<-"  },
            {TokenType.GoTo, @"^GoTo"},

            //aritmetic expressions
            { TokenType.Plus, @"^\+" },
            { TokenType.Minus, @"^-" },
            { TokenType.Multiply, @"^\*" },
            { TokenType.Divide, @"^/" },
            {TokenType.Pow,@"^\*\*"},
            {TokenType.Modulus, @"^%" },

             //boolean expression
            {TokenType.And, @"^&&"},
            {TokenType.Or, @"^||"},
            { TokenType.Equal, @"^==" },
            { TokenType.Greater, @"^>" },
            { TokenType.Less, @"^<" },
            { TokenType.GreaterEqual, @"^>=" },
            { TokenType.LessEqual, @"^<=" },

            //Symbols
            { TokenType.OpenParen, @"^\(" }, 
            { TokenType.CloseParen, @"^\)" },
            { TokenType.OpenBracket, @"^\[" },
            { TokenType.CloseBracket, @"^\]" },
            { TokenType.Comma, @"^," },

        
        };

    }
   
    
    public enum TokenType
    {
        Identifier,
        Number,
        Color,
        AssigmentExpresions,
        GoTo,
        
        //aritmetic expressions
         Plus, Minus, Multiply, Divide,Pow, Modulus,
         //boolean expression
        And, Or, Equal, Less, LessEqual, Greater, GreaterEqual,
         //Symbols
        OpenBracket, CloseBracket, OpenParen, CloseParen, Comma,

        Istructions,
        Functions,
        Desconocido,
    }
  
  
}
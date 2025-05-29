using System.Collections.Generic;
using System.Collections;
public class TokenTypeExtensions
{
    public static readonly Dictionary<TokenType, string> TokenGeneral;
    public static readonly HashSet<string> ColorValue = new HashSet<string>
{
    "Red", "Blue", "Green", "Yellow", "Orange", "Purple", "Black", "White", "Transparent"
};

    static TokenTypeExtensions()
    {
        TokenGeneral = new()
        {
            {TokenType.GoTo, @"^GoTo"},
            { TokenType.Number, @"^\b\d+\b" },
            {TokenType.Identifier,@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ][a-zA-ZñÑáéíóúÁÉÍÓÚ0-9_-]*" },
            {TokenType.String_,"^\"[^\"]*\""},
            {TokenType.AssigmentExpresions, @"^\<-"  },
            

            //aritmetic expressi_ons
            { TokenType.Plus, @"^\+" },
            { TokenType.Minus, @"^-" },
             {TokenType.Pow,@"^\*\*"},
            { TokenType.Multiply, @"^\*" },
            { TokenType.Divide, @"^/" },
            {TokenType.Modulus, @"^%" },

             //boolean expression
            {TokenType.And, @"^&&"},
             { TokenType.Or, @"^\|\|" },
            { TokenType.Equal, @"^==" },
            { TokenType.GreaterEqual, @"^>=" },
            { TokenType.LessEqual, @"^<=" },
             { TokenType.Greater, @"^>" },
            { TokenType.Less, @"^<" },

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
        String_,
        AssigmentExpresions,
        GoTo,

        //aritmetic expressions
        Plus, Minus, Multiply, Divide, Pow, Modulus,
        //boolean expression
        And, Or, Equal, Less, LessEqual, Greater, GreaterEqual,
        //Symbols
        OpenBracket, CloseBracket, OpenParen, CloseParen, Comma,

        EOL,
        EOF,

        Istructions,
        Functions,
        Desconocido,


    }
}
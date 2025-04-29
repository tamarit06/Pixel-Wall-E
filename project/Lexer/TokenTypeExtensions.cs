using System.Collections.Generic;
using System.Collections;
public class TokenTypeExtensions
{
    public static readonly Dictionary<TokenType, string> TokenGeneral;
    
    public static readonly HashSet<string>  InstruccionsValue;
    public static readonly HashSet<string>  FunctionsValue;
    public static readonly HashSet<string>  ColorValue;

    static TokenTypeExtensions()
    {
         TokenGeneral = new()
        {
            { TokenType.Number, @"^\b\d+\b" },
            { TokenType.Identifier, @"\b[A-Z][a-zA-Z]*\b"},
            {TokenType.Color,"^\"[^\"]*\""},
            {TokenType.AssigmentExpresions, @"^\<-"  },
            {TokenType.GoTo, @"^GoTo"},
            {TokenType.VariableLebel,  @"^[a-zA-ZñÑ][a-zA-Z0-9ñÑ-]*$"},


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
        
         InstruccionsValue=new HashSet<string>
        {
            "Spawn", "Color", "Size", "DrawLine", "DrawCircle", "DrawRectangle", "Fill", 
        };

        FunctionsValue=new HashSet<string>
        {
            "GetActualX", "GetActualY", "GetCanvasSize", "GetColorCount", "IsBrushColor", "IsBrushSize", "IsCanvasColor",
        };

        ColorValue=new HashSet<string>
        {
            "Red", "Blue", "Green", "Yellow", "Orange", "Purple", "Black", "White", "Transparent",
        };

    }
   
    
    public enum TokenType
    {
        Identifier,
        Number,
        Color,
        AssigmentExpresions,
        GoTo,
       VariableLebel,
        
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
  
  /*  public enum TokenType
    {
       //keywords
       GoTo,

       //intructions
       Spawn, Color, Size, DrawLine, DrawCircle, DrawRectangle, Fill, 

       //arithmetic expressions
       Sum, Subtraction, Multiplication, Division,Pow, Modulus,

       //boolean expression
       And, Or, Equal, less, lessOrEqual, greather, greatherOrEqual,

       //functions
       GetActualX, GetActualY, GetCanvasSize, GetColorCount, IsBrushColor, IsBrushSize, IsCanvasColor,

       //assigment expresions
       assignment,

        //colors
       Red, Blue, Green, Yellow, Orange, Purple, Black, White, Transparent,

       //Symbols
        Hyphen, OpenBracket, CloseBracket, OpenParenthesis, CloseParenthesis, Comma, DoubleQuote,
    }*/
}
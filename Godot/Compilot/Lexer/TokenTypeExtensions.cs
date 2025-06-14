using System.Collections.Generic;
using System.Collections;

public class TokenTypeExtensions
{
    public static readonly Dictionary<TokenType, string> TokenGeneral;
   public static readonly HashSet<string> ColorValue = new HashSet<string>
{
    "AliceBlue", "AntiqueWhite", "Aqua", "Aquamarine", "Azure", "Beige", "Bisque", "Black", "BlanchedAlmond",
    "Blue", "BlueViolet", "Brown", "Burlywood", "CadetBlue", "Chartreuse", "Chocolate", "Coral",
    "CornflowerBlue", "Cornsilk", "Crimson", "Cyan", "DarkBlue", "DarkCyan", "DarkGoldenrod", "DarkGray",
    "DarkGreen", "DarkKhaki", "DarkMagenta", "DarkOliveGreen", "DarkOrange", "DarkOrchid", "DarkRed",
    "DarkSalmon", "DarkSeaGreen", "DarkSlateBlue", "DarkSlateGray", "DarkTurquoise", "DarkViolet",
    "DeepPink", "DeepSkyBlue", "DimGray", "DodgerBlue", "Firebrick", "FloralWhite", "ForestGreen",
    "Fuchsia", "Gainsboro", "GhostWhite", "Gold", "Goldenrod", "Gray", "Green", "GreenYellow", "Honeydew",
    "HotPink", "IndianRed", "Indigo", "Ivory", "Khaki", "Lavender", "LavenderBlush", "LawnGreen",
    "LemonChiffon", "LightBlue", "LightCoral", "LightCyan", "LightGoldenrod", "LightGray", "LightGreen",
    "LightPink", "LightSalmon", "LightSeaGreen", "LightSkyBlue", "LightSlateGray", "LightSteelBlue",
    "LightYellow", "Lime", "LimeGreen", "Linen", "Magenta", "Maroon", "MediumAquamarine", "MediumBlue",
    "MediumOrchid", "MediumPurple", "MediumSeaGreen", "MediumSlateBlue", "MediumSpringGreen",
    "MediumTurquoise", "MediumVioletRed", "MidnightBlue", "MintCream", "MistyRose", "Moccasin",
    "NavajoWhite", "NavyBlue", "OldLace", "Olive", "OliveDrab", "Orange", "OrangeRed", "Orchid",
    "PaleGoldenrod", "PaleGreen", "PaleTurquoise", "PaleVioletRed", "PapayaWhip", "PeachPuff", "Peru",
    "Pink", "Plum", "PowderBlue", "Purple", "RebeccaPurple", "Red", "RosyBrown", "RoyalBlue",
    "SaddleBrown", "Salmon", "SandyBrown", "SeaGreen", "Seashell", "Sienna", "Silver", "SkyBlue",
    "SlateBlue", "SlateGray", "Snow", "SpringGreen", "SteelBlue", "Tan", "Teal", "Thistle", "Tomato",
    "Transparent", "Turquoise", "Violet", "WebGray", "WebGreen", "WebMaroon", "WebPurple", "Wheat",
    "White", "WhiteSmoke", "Yellow", "YellowGreen"
};


 public static readonly HashSet<string> Instructions = new HashSet<string>
{
    "Spawn", "Color", "Size", "DrawLine", "DrawCircle", "DrawRectangle", "Fill", "MoveWalle"
};

    static TokenTypeExtensions()
    {
        TokenGeneral = new()
        {
            {TokenType.GoTo, @"^GoTo"},
            { TokenType.Number, @"^\b\d+\b" },
            {TokenType.Identifier,@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ][a-zA-ZñÑáéíóúÁÉÍÓÚ0-9_]*" },
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
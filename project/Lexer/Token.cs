using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static TokenTypeExtensions;


public class Token
{
    public TokenType Type { get; }
    public string Lexeme { get; }
    public int Linea { get; }
    public int Posicion { get; }

    public Token(TokenType type, string lexeme, int linea, int posicion)
    {
        Type = type;
        Lexeme = lexeme;
        Linea = linea;
        Posicion = posicion;
    }

    public override string ToString() => $"[{Type}: '{Lexeme}' en línea {Linea}, posición {Posicion}]";
}
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static TokenTypeExtensions;

public class Lexer
{
    private readonly string Input;
    private int Position;
    private int Linea;
    public List<Token> Tokens { get; set; }

    public Lexer(string input)
    {
        Input = input;
        Position = 0;
        Linea = 1;
        Tokens = new List<Token>();
    }

    public List<Token> Tokenize()
    {
        while (Position < Input.Length)
        {
            if (char.IsWhiteSpace(Input[Position]))
            {
                if (Input[Position] == '\n') Linea++;
                Position++;
                continue;
            }

            bool matched = false;

         foreach (var entry in TokenTypeExtensions.TokenGeneral)
{
    var regex = new Regex(entry.Value);
    var match = regex.Match(Input.Substring(Position));

    Console.WriteLine($"Probando patrón: {entry.Key} en '{Input.Substring(Position)}'");

    if (match.Success && match.Index == 0  && match.Length > 0)
    {
        string value = match.Value;
        TokenType type = entry.Key;

        if (entry.Key == TokenType.Identifier)
        {
            if (TokenTypeExtensions.InstruccionsValue.Contains(value))
                type =TokenType.Istructions;
            else if (TokenTypeExtensions.FunctionsValue.Contains(value))
                type = TokenType.Functions;
            else
            {
                type=TokenType.Desconocido;
            }
        }
        else if (entry.Key == TokenType.Color &&TokenTypeExtensions.ColorValue.Contains(value.Trim('"')))
        {
            type = TokenType.Color;
        }

        AddToken(type, value);
        Position += match.Length;
        matched = true;
        break; // solo rompemos si hubo match
    }
}


            if (!matched)
            {
                AddToken(TokenType.Desconocido, Input[Position].ToString());
                Position++;
            }
        }

        return Tokens;
    }

    private void AddToken(TokenType type, string lexeme)
    {
        Tokens.Add(new Token(type, lexeme, Linea, Position));
    }
}
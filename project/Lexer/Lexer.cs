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
                if (Input[Position] == '\n')
                {
                    Linea++;
                    AddToken(TokenType.EOL,"\\n");
                }
                Position++;
                continue;
            }

            bool matched = false;

            foreach (var entry in TokenTypeExtensions.TokenGeneral)
            {
                var regex = new Regex(entry.Value);
                var match = regex.Match(Input.Substring(Position));

                if (match.Success && match.Index == 0  && match.Length > 0)
                {
                    string value = match.Value;
                    TokenType type = entry.Key;

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
        AddToken(TokenType.EOF, "EOF");
        return Tokens;
    }

    private void AddToken(TokenType type, string lexeme)
    {
        Tokens.Add(new Token(type, lexeme, Linea, Position));
    }
}
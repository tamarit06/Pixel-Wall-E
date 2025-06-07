using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static TokenTypeExtensions;

public class Lexer
{
    private readonly string Input;
    private int Position;
    private int Line;
    public List<Token> Tokens { get; private set; }
    public List<ErrorException> Errores { get; } = new();

    public Lexer(string input)
    {
        Input = input;
        Position = 0;
        Line = 1;
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
                    Line++;
                    AddToken(TokenType.EOL, "\\n");
                }
                Position++;
                continue;
            }

            bool matched = false;

            foreach (var entry in TokenTypeExtensions.TokenGeneral)
            {
                var regex = new Regex(entry.Value);
                var match = regex.Match(Input.Substring(Position));

                if (match.Success && match.Index == 0 && match.Length > 0)
                {
                    string value = match.Value;
                    TokenType type = entry.Key;

                    AddToken(type, value);
                    Position += match.Length;
                    matched = true;
                    break;
                }
            }

            if (!matched)
            {
                string ch = Input[Position].ToString();
                Errores.Add(new ErrorException($"Símbolo inválido: '{ch}'", Line, Position));
                Position++; // sigue al próximo carácter
            }
        }

        AddToken(TokenType.EOF, "EOF");
        return Tokens;
    }

    private void AddToken(TokenType type, string lexeme)
    {
         if (type == TokenType.String_)
    {
        lexeme = lexeme.Trim('"');
    }
        Tokens.Add(new Token(type, lexeme, Line, Position));
    }
}

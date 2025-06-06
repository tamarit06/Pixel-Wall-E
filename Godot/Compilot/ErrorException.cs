using System;
public class ErrorException : Exception
{
    public int Line { get; }
    public int Position { get; }

    public ErrorException(string message, int line, int position)
        : base($"{message} (en línea {line}, posición {position})")
    {
        Line = line;
        Position = position;
    }
}

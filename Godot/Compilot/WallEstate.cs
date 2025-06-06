using System;

public class WallEState
{
    public int X { get; set; }
    public int Y { get; set; }
    public string BrushColor { get; set; } = "\"Black\"";
    public int BrushSize { get; set; } = 1;
    public int CanvasSize;

    public string[,] Canvas { get; private set; }
    public Action<int, int, string>? OnPixelPaint;

    public WallEState(int canvasSize)
    {
        CanvasSize = canvasSize;
        Canvas = new string[CanvasSize, CanvasSize];
        for (int i = 0; i < canvasSize; i++)
        {
            for (int j= 0; j < canvasSize; j++)
            {
                Canvas[i, j] = "\"White\"";
            }
        }
    }



    private bool EsValido(int x, int y)
    {
        return x >= 0 && y >= 0 && x < CanvasSize && y < CanvasSize;
    }

    public void DrawLine(int dx, int dy, int length)
    {
        for (int i = 0; i < length; i++)
        {
            if (EsValido(X, Y))
            {
                Canvas[X, Y] = BrushColor;
                OnPixelPaint?.Invoke(X, Y, BrushColor);
            }
            X += dx;
            Y += dy;
        }
    }

    public void FillFrom(int x, int y, string targetColor)
    {
        if (!EsValido(x, y) || Canvas[x, y] != targetColor || Canvas[x, y] == BrushColor)
            return;

        Canvas[x, y] = BrushColor;
        OnPixelPaint?.Invoke(x, y, BrushColor);

        FillFrom(x + 1, y, targetColor);
        FillFrom(x - 1, y, targetColor);
        FillFrom(x, y + 1, targetColor);
        FillFrom(x, y - 1, targetColor);
    }

    public void DrawRectangle(int dx, int dy, int distance, int width, int height)
    {
        int startX = X;
        int startY = Y;

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                int px = startX + col * dx;
                int py = startY + row * dy;

                if (EsValido(px, py))
                {
                    Canvas[px, py] = BrushColor;
                    OnPixelPaint?.Invoke(px, py, BrushColor);
                }
            }
        }

        // Mover el cursor final
        X = startX + dx * distance;
        Y = startY + dy * distance;
    }

    public void DrawCircle(int dx, int dy, int radius)
    {
        int centerX = X;
        int centerY = Y;

        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                if (i * i + j * j <= radius * radius)
                {
                    int px = centerX + i;
                    int py = centerY + j;

                    if (EsValido(px, py))
                    {
                        Canvas[px, py] = BrushColor;
                        OnPixelPaint?.Invoke(px, py, BrushColor);
                    }
                }
            }
        }

        // Mover el cursor final
        X = centerX + dx * radius;
        Y = centerY + dy * radius;
    }
}

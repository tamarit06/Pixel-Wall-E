using System;

public class WallEState
{
    public int X { get; set; }
    public int Y { get; set; }
    public string BrushColor { get; set; } = "Black";
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
            for (int j = 0; j < canvasSize; j++)
            {
                Canvas[i, j] = "White";
            }
        }
    }

    private bool EsValido(int x, int y)
    {
        return x >= 0 && y >= 0 && x < CanvasSize && y < CanvasSize;
    }

    public void SetPixel(int x, int y)
    {
        if (x >= 0 && x < CanvasSize && y >= 0 && y < CanvasSize)
            Canvas[x, y] = BrushColor;
    }
    public void DrawLine(int dx, int dy, int length)
    {
        int offset = BrushSize / 2;
        // Pintar el punto de inicio
        SetBrushPixels(X, Y);

        for (int i = 1; i <=length; i++)
        {
            X += dx;
            Y += dy;
            if (i < length)
            {
                SetBrushPixels(X, Y);
            }

            else
            {
                SetPixel(X, Y);
            }
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
        // 1) Avanza distance pasos
        X += dx * distance;
        Y += dy * distance;
        
        // 2) Calcula semianchos en cada eje (width,height impares o pares funcionan)
        int halfW = width / 2;
        int halfH = height / 2;

        // 3) Dibuja borde superior e inferior
        for (int offsetX = -halfW; offsetX <= halfW; offsetX++)
        {
            // Y fijo en cima y base
            SetBrushPixels(X + offsetX, Y - halfH);  // borde superior
            SetBrushPixels(X + offsetX, Y + halfH);  // borde inferior
        }

        // 4) Dibuja borde izquierdo y derecho
        for (int offsetY = -halfH; offsetY <= halfH; offsetY++)
        {
            SetBrushPixels(X - halfW, Y + offsetY);  // borde izquierdo
            SetBrushPixels(X + halfW, Y + offsetY);  // borde derecho
        }
    }
   public void DrawCircle(int dx, int dy, int radius)
{
    // 1) Avanza radius pasos en la dirección (dx, dy)
    X += dx * radius;
    Y += dy * radius;

    // 2) Dibuja la circunferencia de radio 'radius' centrada en (X, Y)
    MidpointCircle(X, Y, radius);
}

private void MidpointCircle(int cx, int cy, int r)
{
    int x = r+1;
    int y = 0;
    int err = 1 -r;

    while (x >= y)
    {
        // pinta los 8 puntos simétricos
        SetBrushPixels(cx + x, cy + y);
        SetBrushPixels(cx + y, cy + x);
        SetBrushPixels(cx - y, cy + x);
        SetBrushPixels(cx - x, cy + y);
        SetBrushPixels(cx - x, cy - y);
        SetBrushPixels(cx - y, cy - x);
        SetBrushPixels(cx + y, cy - x);
        SetBrushPixels(cx + x, cy - y);

        y++;
        if (err < 0)
        {
            err += 2 * y + 1;
        }
        else
        {
            x--;
            err += 2 * (y - x + 1);
        }
    }
}
    private void SetBrushPixels(int px, int py)
{
    int off = BrushSize / 2;
    for (int dx = -off; dx <= off; dx++)
        for (int dy = -off; dy <= off; dy++)
            SetPixel(px + dx, py + dy);
}
}

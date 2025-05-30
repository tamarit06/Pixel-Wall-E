public class WallEState
{
    public int X { get; set; }
    public int Y { get; set; }
    public string BrushColor { get; set; } = "Transparent";
    public int BrushSize { get; set; } = 1;
    public int CanvasSize { get; set; } = 256;

    public string[,] Canvas { get; private set; }

    public WallEState()
    {
        Canvas = new string[CanvasSize, CanvasSize];
        for (int i = 0; i < CanvasSize; i++)
            for (int j = 0; j < CanvasSize; j++)
                Canvas[i, j] = "\"White\"";//ver esto
    }

    public void SetPixel(int x, int y)
    {
        if (x >= 0 && x < CanvasSize && y >= 0 && y < CanvasSize)
            Canvas[x, y] = BrushColor;
    }

    public void FillFrom(int x, int y, string originalColor)
    {
        if (x < 0 || x >= CanvasSize || y < 0 || y >= CanvasSize) return;
        if (Canvas[x, y] != originalColor || Canvas[x, y] == BrushColor) return;

        Canvas[x, y] = BrushColor;

        FillFrom(x + 1, y, originalColor);
        FillFrom(x - 1, y, originalColor);
        FillFrom(x, y + 1, originalColor);
        FillFrom(x, y - 1, originalColor);
    }

    public void DrawLine(int dx, int dy, int length)
    {
        for (int i = 0; i < length; i++)
        {
            X += dx;
            Y += dy;

            int offset = BrushSize / 2;
            for (int dxBrush = -offset; dxBrush <= offset; dxBrush++)
            {
                for (int dyBrush = -offset; dyBrush <= offset; dyBrush++)
                {
                    SetPixel(X + dxBrush, Y + dyBrush);
                }
            }
        }
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
    int x = r;
    int y = 0;
    int err = 1 - r;

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

// Usa SetBrushPixels para respetar BrushSize impar
private void SetBrushPixels(int px, int py)
{
    int off = BrushSize / 2;
    for (int dx = -off; dx <= off; dx++)
        for (int dy = -off; dy <= off; dy++)
            SetPixel(px + dx, py + dy);
}

}

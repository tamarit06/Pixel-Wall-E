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
                Canvas[i, j] = "White";
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
}

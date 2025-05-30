public static class CanvasPrinter
{
    public static void Print(WallEState state)
    {
        for (int y = 0; y < state.CanvasSize; y++)
        {
            for (int x = 0; x < state.CanvasSize; x++)
            {
                string color = state.Canvas[x, y];
                char c = color switch
                {
                    "White" => '.',
                    "Red" => 'R',
                    "Blue" => 'B',
                    "Black" => '#',
                    "Green" => 'G',
                    "Transparent" => ' ',
                    _ => '*'
                };
                Console.Write(c);
            }
            Console.WriteLine();
        }
        Console.WriteLine($"\nWall-E está en ({state.X}, {state.Y})");
        Console.WriteLine($"Color: {state.BrushColor}, Tamaño: {state.BrushSize}");
    }
}

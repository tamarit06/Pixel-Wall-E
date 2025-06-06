using Godot;
using System;

public partial class Main : Control
{
    ColorRect colorRect;
    CodeEdit codeEdit;
    TextEdit textEdit;
    Button runButton;
    Button saveButton;
    Button loadButton;
    SpinBox spinBox;
    FileDialog fileDialogSave;
    FileDialog fileDialogLoad;
    public int Dimension = 32;
    public const int BoardPixelsSize = 800;
    Image image;
    ImageTexture texture;
    TextureRect textureRect;
    public override void _Ready()
    {
        colorRect = GetNode<ColorRect>("ColorRect");
        codeEdit = GetNode<CodeEdit>("CodeEdit");
        textEdit = GetNode<TextEdit>("TextEdit");
        textureRect = GetNode<TextureRect>("TextureRect");
        runButton = GetNode<Button>("RunButton");
        saveButton = GetNode<Button>("SaveButton");
        loadButton = GetNode<Button>("LoadButton");
        spinBox = GetNode<SpinBox>("SpinBox");

        runButton.Pressed += OnRunPressed;
        saveButton.Pressed += OnSavePressed;
        loadButton.Pressed += OnLoadPressed;
        spinBox.ValueChanged += OnSpinBoxValueChanged;

        // Crear y configurar el FileDialog para guardar archivos
        fileDialogSave = new FileDialog();
        // Usamos Filesystem en lugar de User
        fileDialogSave.Access = FileDialog.AccessEnum.Filesystem;
        // No se asigna la propiedad Mode, ya que produce error en esta versión
        fileDialogSave.Filters = new string[] { "*.gw" };
        AddChild(fileDialogSave);
        fileDialogSave.FileSelected += _OnFileDialogSaveFileSelected;

        // Crear y configurar el FileDialog para cargar archivos
        fileDialogLoad = new FileDialog();
        fileDialogLoad.Access = FileDialog.AccessEnum.Filesystem;
        // No se asigna la propiedad Mode, ya que produce error en esta versión
        fileDialogLoad.Filters = new string[] { "*.gw" };
        AddChild(fileDialogLoad);
        fileDialogLoad.FileSelected += _OnFileDialogSaveFileSelected;

        InicializarCanvas();
        PintarCuadrícula();
    }

    public void OnRunPressed()
    {
        Compiler();
    }

    public void OnSavePressed()
    {

    }

    public void OnLoadPressed()
    {

    }
    public void OnSpinBoxValueChanged(double newValue)
    {
        Reset();
        Dimension = (int)newValue;
        PintarCuadrícula();
    }
    public void _OnFileDialogSaveFileSelected(string path)
    {
        GD.Print("Guardar Código");
        fileDialogSave.PopupCentered();
        fileDialogSave.Size = new Vector2I(600, 400); // ancho x alto
        fileDialogLoad.Size = new Vector2I(600, 400);
    }
    public void _OnFileDialogLoadFileSelected(string path)
    {
        GD.Print("Cargar Código");
        fileDialogLoad.PopupCentered();
        fileDialogSave.Size = new Vector2I(600, 400); // ancho x alto
        fileDialogLoad.Size = new Vector2I(600, 400);
    }
    private void InicializarCanvas()
    {
        image = Image.CreateEmpty(BoardPixelsSize, BoardPixelsSize, false, Image.Format.Rgba8);
        image.Fill(Colors.White);
        texture = ImageTexture.CreateFromImage(image);
        textureRect.Texture = texture;
    }

    // Función para pintar una celda completa en el canvas
    public void PintarCelda(int cellX, int cellY, Color color)
    {
        float cellSize = (float)BoardPixelsSize / Dimension;
        int startX = (int)(cellX * cellSize);
        int startY = (int)(cellY * cellSize);
        int cellPixelSize = (int)cellSize;

        for (int x = startX; x < startX + cellPixelSize; x++)
        {
            for (int y = startY; y < startY + cellPixelSize; y++)
            {
                if (x < BoardPixelsSize && y < BoardPixelsSize)
                {
                    image.SetPixel(x, y, color);
                }
            }
        }
        texture.Update(image);
    }
    public void PintarCuadrícula()
    {
        int divisions = Dimension;
        float cellSizeF = (float)BoardPixelsSize / divisions;

        for (int i = 0; i <= divisions; i++)
        {
            int pos = (int)(i * cellSizeF);

            // Si pos es igual a BoardPixelSize, lo ajustamos al último índice válido (899).
            if (pos >= BoardPixelsSize)
                pos = BoardPixelsSize - 1;

            // Línea vertical
            for (int y = 0; y < BoardPixelsSize; y++)
                image.SetPixel(pos, y, Colors.Black);

            // Línea horizontal
            for (int x = 0; x < BoardPixelsSize; x++)
                image.SetPixel(x, pos, Colors.Black);
        }

        texture.Update(image);
    }
    private void Reset()
    {
        image.Fill(Colors.White);
        texture.Update(image);
    }

    public void Compiler()
    {
        string code = codeEdit.GetText();
        Lexer lexer = new Lexer(code);
        lexer.Tokenize();

        var parser = new Parser(lexer);
        parser.Parsind();
        WallEState canvas = new WallEState(Dimension);

        var evaluator = new Evaluate(canvas);
        evaluator.EvaluateProgram(parser.Nodos);

        //ver q hacer con los errores

        Reset();
        Print(canvas);

    }

    public void Print(WallEState canvas)
    {
        // Asegurarnos de que el estado lógico coincida con la dimensión actual
       // canvas.CanvasSize = Dimension;

        // Limpiar el canvas visual antes de pintar
        Reset();

        // Recorrer cada celda lógica y pintarla
        for (int x = 0; x < Dimension; x++)
        {
            for (int y = 0; y < Dimension; y++)
            {
                // Obtener el nombre del color desde el estado lógico
                string color = canvas.Canvas[x, y];
                GD.Print(color);
                // Convertirlo a un Color de Godot
                if (color == "\"Transparent\"") continue;
                Color godotColor = ConvertirColor(color);
                // Pintar la celda en pantalla
                PintarCelda(x, y, new Color(godotColor));
            }

        }
        PintarCuadrícula();

    }

    private Color ConvertirColor(string colorName)
    {
        
        return colorName switch
        {
            "\"Red\"" => Colors.Red,
            "\"Blue\"" => Colors.Blue,
            "\"Green\"" => Colors.Green,
            "\"Yellow\"" => Colors.Yellow,
            "\"Orange\"" => Colors.Black,
            "\"White\"" => Colors.White,
            "\"Purple\"" => Colors.Purple,
            "\"Black\"" => Colors.Black,
            _ => throw new ArgumentException($"Color no soportado '{colorName}'")
        };

    }
}

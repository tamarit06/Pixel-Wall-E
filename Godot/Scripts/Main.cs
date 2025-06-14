using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Main : Control
{
	ColorRect colorRect;
	CodeEdit codeEdit;
	TextEdit textEdit;
	Button runButton;
	Button saveButton;
	Button loadButton;
	Button resetButton;
	SpinBox spinBox;
	FileDialog fileDialogSave;
	FileDialog fileDialogLoad;
	public int Dimension = 32;
	public const int BoardPixelsSize = 900;
	Image image;
	ImageTexture texture;
	TextureRect textureRect;
	TextureRect walle;
	public override void _Ready()
	{
		colorRect = GetNode<ColorRect>("ColorRect");
		codeEdit = GetNode<CodeEdit>("CodeEdit");
		textEdit = GetNode<TextEdit>("TextEdit");
		textureRect = GetNode<TextureRect>("TextureRect");
		runButton = GetNode<Button>("RunButton");
		saveButton = GetNode<Button>("SaveButton");
		loadButton = GetNode<Button>("LoadButton");
		resetButton = GetNode<Button>("ResetButton");
		spinBox = GetNode<SpinBox>("SpinBox");
		walle = GetNode<TextureRect>("TextureRect/Walle");
		
		runButton.Pressed += OnRunPressed;
		saveButton.Pressed += OnSavePressed;
		loadButton.Pressed += OnLoadPressed;
		resetButton.Pressed += OnResetPressed;
		spinBox.ValueChanged += OnSpinBoxValueChanged;

		// Crear y configurar el FileDialog para guardar archivos
		fileDialogSave = new FileDialog();
		// Usamos Filesystem en lugar de User
		fileDialogSave.Access = FileDialog.AccessEnum.Filesystem;
		// No se asigna la propiedad Mode, ya que produce error en esta versión
		fileDialogSave.FileMode = FileDialog.FileModeEnum.SaveFile;
		fileDialogSave.Filters = new string[] { "*.gw" };
		AddChild(fileDialogSave);
		fileDialogSave.FileSelected += _OnFileDialogSaveFileSelected;

		// Crear y configurar el FileDialog para cargar archivos
		fileDialogLoad = new FileDialog();
		fileDialogLoad.Access = FileDialog.AccessEnum.Filesystem;
		fileDialogLoad.FileMode = FileDialog.FileModeEnum.OpenAny;
		// No se asigna la propiedad Mode, ya que produce error en esta versión
		fileDialogLoad.Filters = new string[] { "*.gw" };
		AddChild(fileDialogLoad);
		fileDialogLoad.FileSelected += _OnFileDialogLoadFileSelected;

		InicializarCanvas();
		PintarCuadrícula();
	}

	public void OnRunPressed()
	{
		Compiler();
	}

	public void OnSavePressed()
	{
        fileDialogSave.PopupCentered();
        fileDialogSave.Size = new Vector2I(600, 400); 
	}

	public void OnLoadPressed()
	{
        fileDialogLoad.PopupCentered();
        fileDialogLoad.Size = new Vector2I(600, 400);

	}
	public void OnResetPressed()
	{
		Reset();
		PintarCuadrícula();
	}
	public void OnSpinBoxValueChanged(double newValue)
	{
		Reset();
		Dimension = (int)newValue;
		PintarCuadrícula();
	}
	  private void _OnFileDialogSaveFileSelected(string ruta)
    {
        GuardarArchivo(ruta);
        PintarCuadrícula();
        GD.Print("Archivo guardado en: " + ruta);
    }
    private void _OnFileDialogLoadFileSelected(string ruta)
    {
        CargarArchivo(ruta);
        PintarCuadrícula();
        GD.Print("Archivo cargado desde: " + ruta);
    }
    // Función para guardar el contenido del editor en un archivo
    private void GuardarArchivo(string ruta)
    {
        var archivo = Godot.FileAccess.Open(ruta, Godot.FileAccess.ModeFlags.Write);
        archivo.StoreString(codeEdit.GetText());
        archivo.Close();
    }
    // Función para cargar el contenido de un archivo en el editor
    private void CargarArchivo(string ruta)
    {
        Reset();
        var archivo = Godot.FileAccess.Open(ruta, Godot.FileAccess.ModeFlags.Read);
        string contenido = archivo.GetAsText();
        archivo.Close();
        codeEdit.SetText(contenido);
    }
	private void InicializarCanvas()
	{
		image = Image.CreateEmpty(BoardPixelsSize, BoardPixelsSize, false, Image.Format.Rgba8);
		image.Fill(Colors.White);
		texture = ImageTexture.CreateFromImage(image);
		textureRect.Texture = texture;
		textureRect.Size = new Vector2(BoardPixelsSize, BoardPixelsSize);
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
        Color gridColor = Colors.Black;
        int divs = Dimension;
        int baseW =  BoardPixelsSize / divs;
        int remW =  BoardPixelsSize % divs;
        int baseH =  BoardPixelsSize / divs;
        int remH =  BoardPixelsSize % divs;

        // Acumulamos posiciones de línea en X
        int xPos = 0;
        for (int i = 0; i <= divs; i++)
        {
            // Dibujo una línea vertical 1px de ancho en xPos
            var lineV = new Rect2I(new Vector2I(xPos, 0), new Vector2I(1,  BoardPixelsSize));
            image.FillRect(lineV, gridColor);

            // Incremento xPos: para i<divs avanzo ancho de celda i
            if (i < divs)
                xPos += (i < remW) ? (baseW + 1) : baseW;
        }

        // Acumulamos posiciones de línea en Y
        int yPos = 0;
        for (int j = 0; j <= divs; j++)
        {
            var lineH = new Rect2I(new Vector2I(0, yPos), new Vector2I( BoardPixelsSize, 1));
            image.FillRect(lineH, gridColor);

            if (j < divs)
                yPos += (j < remH) ? (baseH + 1) : baseH;
        }

		texture.Update(image);
	}
	private void Reset()
	{
		image.Fill(Colors.White);
		texture.Update(image);
		walle.Visible = false;

	}

	public void Compiler()
{
		textEdit.Clear();
    string code = codeEdit.GetText();

    // 1) LEXER
    var lexer = new Lexer(code);
    lexer.Tokenize();

    // Si hay errores de lexer, muéstralos y aborta
    if (lexer.Errores.Count > 0)
    {
        ShowErrors(lexer.Errores.Select(e => e.Message));
        return;
    }

    // 2) PARSER
    var parser = new Parser(lexer);
    parser.Parsind();

    // Si hay errores de parser, muéstralos y aborta
    if (parser.Errores.Count > 0)
    {
        ShowErrors(parser.Errores.Select(e=>e.Message));
        return;
    }

    // 3) EVALUADOR
    var canvas    = new WallEState(Dimension);
    var evaluator = new Evaluate(canvas);
    evaluator.EvaluateProgram(parser.Nodos);

    // Si hay errores de evaluación, muéstralos y aborta
    if (evaluator.ErroresEvaluacion.Count > 0)
    {
        ShowErrors(evaluator.ErroresEvaluacion.Select(e=>e.Message));
        return;
    }

    // 4) Si todo OK, limpia y pinta
    Reset();
    Print(canvas);
}
private void ShowErrors(IEnumerable<string> errores)
{
    textEdit.Text = "";      // vaciar consola
    foreach (var msg in errores)
        textEdit.Text += $"• {msg}\n";
}

	private void Print(WallEState canvas)
    {
        var pixels = canvas.Canvas;
        int divs = Dimension;
        int baseW =  BoardPixelsSize / divs;
        int remW =  BoardPixelsSize % divs;
        int baseH =  BoardPixelsSize / divs;
        int remH =  BoardPixelsSize % divs;

        int yPos = 0;
        for (int y = 0; y < divs; y++)
        {
            // Altura de esta fila:
            int rowHeight = (y < remH) ? baseH + 1 : baseH;

            int xPos = 0;
            for (int x = 0; x < divs; x++)
            {
                int colWidth = (x < remW) ? baseW + 1 : baseW;
                var rect = new Rect2I(new Vector2I(xPos, yPos), new Vector2I(colWidth, rowHeight));

                string color = pixels[x, y].ToString();
				if (canvas.X == x && canvas.Y == y)
				{
					var iconTexture = GD.Load<Texture2D>("res://Imagenes/WallE.png");
					if (iconTexture is null)
						GD.Print("No se cargo la imagen");

					walle.Texture = iconTexture;

					walle.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
					walle.StretchMode = TextureRect.StretchModeEnum.Scale;


					walle.Position = new Vector2(xPos, yPos);

					walle.Size = new Vector2(colWidth, rowHeight);
					walle.Visible = true;

					GD.Print($"Posicion del rectangulo: ({rect.Position.X},{rect.Position.Y})");
					GD.Print($"Posición de Wall-E: {walle.Position.X},{walle.Position.Y}");
					GD.Print($"Walle: ({y},{x})");
                    GD.Print($"Tamanno del texturerect: ({textureRect.Size.X},{textureRect.Size.Y})");
					
                    
                }
				if (color != "Transparent")
				{
					image.FillRect(rect, new Color(color));
				}

                xPos += colWidth;
            }

            yPos += rowHeight;
        }

        texture.Update(image);
        textureRect.Texture = texture;
        PintarCuadrícula();
    }
	
}

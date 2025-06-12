using Godot;
using System;

public partial class Start : Node2D
{
    Button exit;
    Button start;
    AudioStreamPlayer2D audioStreamPlayer2D;

    public override void _Ready()
    {
        exit = GetNode<Button>("Exit");
        start = GetNode<Button>("Start");
        audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        audioStreamPlayer2D.Play();

        exit.Pressed += OnExitPressed;
        start.Pressed += OnStartPressed;
    }

    public void OnExitPressed()
    {
        GetTree().Quit();
    }

    public void OnStartPressed()
    {
        GetTree().ChangeSceneToFile("res://Scene/main.tscn");
    }
}

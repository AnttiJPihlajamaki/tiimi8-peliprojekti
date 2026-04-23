using Godot;
using System;

public partial class Fullscreen : Node2D
{
    public override void _Ready()
    {
        DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
    }
}

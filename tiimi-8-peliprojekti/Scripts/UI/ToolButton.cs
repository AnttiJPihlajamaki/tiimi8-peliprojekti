using Godot;
using System;

public partial class ToolButton : Button
{
    [Export] private int toolInteger;
    public override void _Ready()
    {
        Pressed += ChangeTool;
    }

    public void ChangeTool()
    {

    }
}

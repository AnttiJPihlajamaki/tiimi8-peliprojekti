using Godot;
using System;

public partial class SpawnAlienTool : Tool
{
	[Export] public PackedScene _Alien;
	[Export] private Aquarium _Aquarium;

    public override void ToolFunction()
    {
		//spawnAlien();
    }
	public override void ToolIncrease()
	{

	}

	public override void ToolDecrease()
	{

	}


	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}

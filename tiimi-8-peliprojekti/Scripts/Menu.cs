using Godot;
using System;

public partial class Menu : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("PlayButton").Pressed += PlayButtonPressed;
	}

	private void PlayButtonPressed()
	{
  		var nextScene = (PackedScene)ResourceLoader.Load("res://Scenes/Aquarium Scene.tscn");
    	GetTree().ChangeSceneToPacked(nextScene);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

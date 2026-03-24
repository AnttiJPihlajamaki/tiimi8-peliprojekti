using Godot;
using System;

public partial class QuitGameButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += QuitGame;
	}

	private void QuitGame()
	{
		GetTree().Quit();
	}
}

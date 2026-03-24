using Godot;
using System;

public partial class ReloadSceneButton : Button
{
	public override void _Ready()
	{
		Pressed +=  ReloadScene;
	}
	private void  ReloadScene()
	{
    	GetTree().ReloadCurrentScene();
	}
}

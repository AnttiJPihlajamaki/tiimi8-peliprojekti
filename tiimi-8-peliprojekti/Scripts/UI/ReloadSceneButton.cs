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
		GameManager.Instance.Reset();
    	GetTree().ReloadCurrentScene();
	}
}

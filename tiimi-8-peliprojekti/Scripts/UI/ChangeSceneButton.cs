using Godot;
using System;

public partial class ChangeSceneButton : Button
{
	[Export] private GameScene scene;
	public override void _Ready()
	{
		Pressed +=  ChangeToScene;
	}
	private void  ChangeToScene()
	{
		var nextScene = (PackedScene)ResourceLoader.Load(scene.ToPathString());
    	GetTree().ChangeSceneToPacked(nextScene);
	}
}

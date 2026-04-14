using System;
using Godot;

[Serializable]
public enum GameScene
{
		MainMenuScene,
		AquariumScene,
		TutorialScene,
}

public static class SceneConfig // String references for all different inputs
{
	public static readonly string[] Scenes =
    [
        new("res://Scenes/MainMenuScene.tscn"),
		new("res://Scenes/AquariumScene.tscn"),
		new("res://Scenes/Tutorial.tscn"),
	];

	public static string ToPathString(this GameScene scene) => Scenes[(int)scene];

}

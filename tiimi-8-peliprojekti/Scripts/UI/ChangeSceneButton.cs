using Godot;
using System;

public partial class ChangeSceneButton : Button
{
	[Export] private ColorRect _blackFadeRect;
	[Export] private GameScene scene;
	public override void _Ready()
	{
		_blackFadeRect.Modulate = new Color(0, 0, 0, 0);
		Pressed +=  ChangeToScene;
	}
	private async void ChangeToScene()
	{
		Tween fadeTween = CreateTween(); // Tween to fade blackFadeRect
		fadeTween.TweenProperty(_blackFadeRect, "modulate:a", 1f, 1f);
		await ToSignal(fadeTween, "finished");

		var nextScene = (PackedScene)ResourceLoader.Load(scene.ToPathString());
    	GetTree().ChangeSceneToPacked(nextScene);
		GameManager.Instance.Reset();
	}
}

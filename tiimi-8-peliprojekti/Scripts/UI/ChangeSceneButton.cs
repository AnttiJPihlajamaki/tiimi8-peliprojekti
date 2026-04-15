using Godot;
using System;

public partial class ChangeSceneButton : Button
{
	[Export] private ColorRect _blackFadeRect;
	[Export] private GameScene _scene;
	public override async void _Ready()
	{
		Pressed +=  ChangeToScene;

		_blackFadeRect.Visible = true;
		_blackFadeRect.MouseFilter = MouseFilterEnum.Stop;

		Tween fadeTween = CreateTween(); // Tween to fade blackFadeRect
		fadeTween.TweenProperty(_blackFadeRect, "modulate:a", 0f, 1f);
		await ToSignal(fadeTween, "finished");

		_blackFadeRect.Visible = false;
		_blackFadeRect.MouseFilter = MouseFilterEnum.Ignore;

		GameManager.Instance.PauseGame(false);
	}
	private async void ChangeToScene()
	{
		GameManager.Instance.PauseGame(true);

		_blackFadeRect.Visible = true;
		_blackFadeRect.MouseFilter = MouseFilterEnum.Stop;

		Tween fadeTween = CreateTween(); // Tween to fade blackFadeRect
		fadeTween.TweenProperty(_blackFadeRect, "modulate:a", 1f, 1f);
		await ToSignal(fadeTween, "finished");

		var nextScene = (PackedScene)ResourceLoader.Load(_scene.ToPathString());
    	GetTree().ChangeSceneToPacked(nextScene);

		GameManager.Instance.Reset();
		PlaySceneMusic();
	}

	private void PlaySceneMusic()
	{
		if(_scene == GameScene.AquariumScene)
		{
			AudioManager.Instance.PlayMusic("Aquarium");
		}
		else if(_scene == GameScene.MainMenuScene)
		{
			AudioManager.Instance.PlayMusic("Menu");
		}
	}
}

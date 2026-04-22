using Godot;
using System;

public partial class ReloadSceneButton : Button
{
	[Export] private ColorRect _blackFadeRect;
	public override void _Ready()
	{
		Pressed +=  ReloadScene;
	}
	private async void ReloadScene()
	{
		AudioManager.Instance.PlaySound("Pop");
		_blackFadeRect.Visible = true;
		_blackFadeRect.MouseFilter = MouseFilterEnum.Stop;

		Tween fadeTween = CreateTween(); // Tween to fade blackFadeRect
		fadeTween.TweenProperty(_blackFadeRect, "modulate:a", 1f, 1f);
		await ToSignal(fadeTween, "finished");
		GameManager.Instance.Reset();
    	GetTree().ReloadCurrentScene();
	}
}

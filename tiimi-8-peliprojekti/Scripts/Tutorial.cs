using Godot;
using System;

public partial class Tutorial : Node2D
{
[Export] private ColorRect _blackFadeRect;
[Export] private Control _TutorialBox;
[Export] private Label _tutorialText;
private int _currentSlide = 0;

	public override void _Ready()
	{
		_blackFadeRect.Modulate = new Color(0, 0, 0, 1); // Start with ColorRect black
		StartIntro();
	}

	private async void StartIntro()
	{
		Tween fadeTween = CreateTween(); // Tween to fade blackFadeRect
		fadeTween.TweenProperty(_blackFadeRect, "modulate:a", 0f, 1f);
		await ToSignal(fadeTween, "finished");

		_tutorialText.Text = "This is your new aquarium shop!\nPlease step inside, it's going to be a busy first day.";
		Tween slideTween = CreateTween(); // Tween to slide tutorial text
		slideTween.TweenProperty(_TutorialBox, "position:y", 440f, 0.5f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quart);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventScreenTouch screenTouch)
		{
			if (screenTouch.IsReleased())
			{
				NextSlide();
			}
		}
	}

	private async void NextSlide()
	{
		_currentSlide++;
		switch (_currentSlide)
		{
			case 1:
				Tween slideDown = CreateTween();
				slideDown.TweenProperty(_TutorialBox, "position:y", 800f, 0.5f).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quart);
				await ToSignal(slideDown, "finished");

				Tween fadeBlack = CreateTween();
				fadeBlack.TweenProperty(_blackFadeRect, "modulate:a", 1f, 1f);
				await ToSignal(fadeBlack, "finished");

				break;
			case 2:
				
				break;
			case 3:
				
				break;
			case 4:
				
				break;
			case 5:
				
				break;
			default:
				GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://Scenes/AquariumScene.tscn"));
				GameManager.Instance.Reset();
				break;
		}
	}

	public override void _Process(double delta)
	{
	
	}
}
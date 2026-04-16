using Godot;
using System;

public partial class Tutorial : Node2D
{
[Export] private ColorRect _introFadeRect;
[Export] private Control _TutorialBox;
[Export] private Control _TutorialBox2;
[Export] private Control _TutorialBox3;
[Export] private Label _tutorialText;
[Export] private Sprite2D _introImage;
[Export] private Sprite2D _introPhone;
[Export] private AudioStreamPlayer2D _introSound;
private int _currentSlide = 0;
private bool _inputEnabled = false;
private String _localisedmessage;

	public override void _Ready()
	{
		GameManager.Instance.PauseGame(false);
		_introFadeRect.Color = new Color(0, 0, 0, 1);
		_introSound.Play();
		StartIntro();
	}

	private async void StartIntro()
	{
		Tween introFadeTween = CreateTween(); // Tween to fade blackFadeRect
		introFadeTween.TweenProperty(_introFadeRect, "modulate:a", 0f, 1.5f);
		await ToSignal(introFadeTween, "finished");

		_localisedmessage = Tr("TINTRODUCTION");
		_tutorialText.Text = string.Format(_localisedmessage);  // welcome to your aquarium päl
		Tween slideTween = CreateTween(); // Tween to slide tutorial text
		slideTween.TweenProperty(_TutorialBox, "position:y", 440f, 0.5f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quart);
		await ToSignal(slideTween, "finished");
		_inputEnabled = true;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!_inputEnabled) return;

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
			{
				_introSound.Stop();
				_inputEnabled = false;
				Tween slideTween = CreateTween();
				slideTween.TweenProperty(_TutorialBox, "position:y", 800f, 0.5f).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quart);
				await ToSignal(slideTween, "finished");

				Tween fadeBlack = CreateTween();
				fadeBlack.TweenProperty(_introFadeRect, "modulate:a", 1f, 1f);
				await ToSignal(fadeBlack, "finished");
				_introPhone.Visible = true;

				_introImage.Visible = false;
				AudioManager.Instance.PlaySound("PHONERING"); // PHONE RINGING SOUND HERE
				fadeBlack = CreateTween();
				fadeBlack.TweenProperty(_introFadeRect, "modulate:a", 0f, 3f);
				await ToSignal(fadeBlack, "finished");
				_inputEnabled = true;

				break;
			}
			case 2:
			{
				_inputEnabled = false;
				AudioManager.Instance.PlaySound("PHONEPICKUP"); // PHONE PICKED UP SOUND HERE
				AudioManager.Instance.PlayMusic("Introcall");
				Tween slideTween = CreateTween();
				_tutorialText.Text = "TCALL1";
				slideTween.TweenProperty(_TutorialBox, "position:y", 440f, 0.7f).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quart);
				await ToSignal(slideTween, "finished");
				AudioManager.Instance.PlaySound("PHONEBABBLE"); // PÄLÄ PÄLÄ ÄÄNI PUHELIN
				_inputEnabled = true;
				break;
			}
			case 3:
			{
				_inputEnabled = false;
				_tutorialText.Text = "TCALL2"; // explain tools vaguely
				AudioManager.Instance.PlaySound("PHONEBABBLE"); // PÄLÄ PÄLÄ ÄÄNI PUHELIN
				Tween slideTween = CreateTween();
				slideTween.TweenProperty(_TutorialBox2, "position:y", 30f, 1.7f).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quart);
				await ToSignal(slideTween, "finished");

				_inputEnabled = true;
				break;
			}
			case 4:
			{
				_tutorialText.Text = "TCALL3";  // explain the feeding tool
				AudioManager.Instance.PlaySound("PHONEBABBLE");
				break;
			}
			case 5:
			{
				_tutorialText.Text = "TCALL4";  // explain shop
				AudioManager.Instance.PlaySound("PHONEBABBLE");
				break;
			}
			case 6:
			{
				_tutorialText.Text = "TCALL5";  // explain oxygenator
				AudioManager.Instance.PlaySound("PHONEBABBLE");
				break;
			}
			case 7:
			{
				Tween slideTween = CreateTween();
				slideTween.TweenProperty(_TutorialBox2, "position:y", -447f, 1.7f).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quart);
				_tutorialText.Text = "TCALL6";  // lasergun
				break;
			}
			case 8:
			{
				AudioManager.Instance.PlaySound("PHONEBABBLE");
				Tween slideTween = CreateTween();
				slideTween.TweenProperty(_TutorialBox3, "position:y", 68f, 1.7f).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quart);
				_tutorialText.Text = "TCALL7";  // explain lasergun
				break;
			}
			case 9:
			{
				_tutorialText.Text = "TCALL8"; // before moving to game scene
				AudioManager.Instance.PlaySound("PHONEBABBLE");
				break;
			}
			case 10:
			{
				Tween introFadeTween = CreateTween(); // Tween to fade blackFadeRect
				introFadeTween.TweenProperty(_introFadeRect, "modulate:a", 1f, 3f);
				await ToSignal(introFadeTween, "finished");
				_currentSlide++;
				break;
			}
			default:
				AudioManager.Instance.StopMusic();
				GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://Scenes/AquariumScene.tscn"));
				GameManager.Instance.Reset();
				break;
		}
	}

	public override void _Process(double delta)
	{

	}
}
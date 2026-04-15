using Godot;
using System;

public partial class LanguageButtons : Node
{
	[Export] private Button _enButton;
	[Export] private Button _fiButton;
	[Export] private Button _seButton;

    public override void _Ready()
	{
		_enButton.Pressed += OnEnPressed;
		_fiButton.Pressed += OnFiPressed;
		_seButton.Pressed += OnSePressed;
	}


	private void OnEnPressed()
	{
		TranslationServer.SetLocale("en");
	}
	private void OnFiPressed()
	{
		TranslationServer.SetLocale("fi");
	}
	private void OnSePressed()
	{
		TranslationServer.SetLocale("se");
	}
}

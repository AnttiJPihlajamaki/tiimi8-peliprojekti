using Godot;
using System;

public partial class LanguageButtons : Node
{
	[Export] private Button _enButton;
	[Export] private Button _fiButton;
	[Export] private Button _seButton;

    public override void _Ready()
	{
		LoadSettings();

		_enButton.Pressed += OnEnPressed;
		_fiButton.Pressed += OnFiPressed;
		_seButton.Pressed += OnSePressed;
	}


	private void OnEnPressed()
	{
		AudioManager.Instance.PlaySound("Pop");
		TranslationServer.SetLocale("en");
		SaveSettings();
	}
	private void OnFiPressed()
	{
		AudioManager.Instance.PlaySound("Pop");
		TranslationServer.SetLocale("fi");
		SaveSettings();
	}
	private void OnSePressed()
	{
		AudioManager.Instance.PlaySound("Pop");
		TranslationServer.SetLocale("se");
		SaveSettings();
	}
	

	private void SaveSettings()
	{
		ConfigFile settingsFile = new();
		Error loadError = settingsFile.Load(SettingsConfig.SETTING_PATH);
		if(loadError != Error.Ok)
		{
			settingsFile = new();
		}
		settingsFile.SetValue(SettingsConfig.LANGUAGE_SETTINGS, SettingsConfig.LANGUAGE_SETTINGS, TranslationServer.GetLocale());

		settingsFile.Save(SettingsConfig.SETTING_PATH);
	}

	private void LoadSettings()
	{
		ConfigFile settingsFile = new();
		Error loadError = settingsFile.Load(SettingsConfig.SETTING_PATH);

		string locale = "en";

		if(loadError == Error.Ok)
		{
			locale = (string)settingsFile.GetValue(SettingsConfig.LANGUAGE_SETTINGS, SettingsConfig.LANGUAGE_SETTINGS, "en");
		}
		TranslationServer.SetLocale(locale);

		if(locale == "en")
		{
			_enButton.ButtonPressed = true;
		}
		else if(locale == "fi")
		{
			_fiButton.ButtonPressed = true;
		}
		else if(locale == "se")
		{
			_seButton.ButtonPressed = true;
		}
	}
}

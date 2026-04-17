using Godot;
using System;

public partial class VolumeSettings : Node
{
	[Export] private HSlider masterSlider;
	[Export] private HSlider musicSlider;
	[Export] private HSlider effectsSlider;

    public override void _Ready()
	{
		LoadSettings();

		masterSlider.ValueChanged += OnMasterVolumeChanged;
		musicSlider.ValueChanged += OnMusicVolumeChanged;
		effectsSlider.ValueChanged += OnEffectsVolumeChanged;

		SetSliderValueToBus(masterSlider, SettingsConfig.MASTER_BUS);
		SetSliderValueToBus(musicSlider, SettingsConfig.MUSIC_BUS);
		SetSliderValueToBus(effectsSlider, SettingsConfig.SFX_BUS);
	}

	private void OnMasterVolumeChanged(double value)
	{
		SetBusVolume(SettingsConfig.MASTER_BUS, value);
	}

	private void OnMusicVolumeChanged(double value)
	{
		SetBusVolume(SettingsConfig.MUSIC_BUS, value);
	}

	private void OnEffectsVolumeChanged(double value)
	{
		SetBusVolume(SettingsConfig.SFX_BUS, value);
	}

	private void SetBusVolume(string name, double value)
	{
		int index = AudioServer.GetBusIndex(name);

		AudioServer.SetBusVolumeLinear(index, (float)value);
		
		SaveSettings();
	}

	private static void SetSliderValueToBus(HSlider slider, string name)
	{
		int index = AudioServer.GetBusIndex(name);
		slider.Value =  AudioServer.GetBusVolumeLinear(index);
	}

	private void SaveSettings()
	{
		ConfigFile settingsFile = new();
		Error loadError = settingsFile.Load(SettingsConfig.SETTING_PATH);
		if(loadError != Error.Ok)
		{
			settingsFile = new();
		}
		string section = SettingsConfig.AUDIO_SETTINGS;

		settingsFile.SetValue(section, SettingsConfig.MASTER_BUS, masterSlider.Value);
		settingsFile.SetValue(section, SettingsConfig.MUSIC_BUS, musicSlider.Value);
		settingsFile.SetValue(section, SettingsConfig.SFX_BUS, effectsSlider.Value);

		settingsFile.Save(SettingsConfig.SETTING_PATH);
	}

	private void LoadSettings()
	{
		float masterVolume = .5f;
		float musicVolume = .5f;
		float sfxVolume = .5f;

		ConfigFile settingsFile = new();
		Error loadError = settingsFile.Load(SettingsConfig.SETTING_PATH);
		if(loadError == Error.Ok)
		{
			string section = SettingsConfig.AUDIO_SETTINGS;
			masterVolume = (float)settingsFile.GetValue(section, SettingsConfig.MASTER_BUS, masterVolume);
		 	musicVolume = (float)settingsFile.GetValue(section, SettingsConfig.MUSIC_BUS, musicVolume);
			sfxVolume = (float)settingsFile.GetValue(section, SettingsConfig.SFX_BUS, sfxVolume);
		}

		SetBusVolume(SettingsConfig.MASTER_BUS, masterVolume);
		SetBusVolume(SettingsConfig.MUSIC_BUS, musicVolume);
		SetBusVolume(SettingsConfig.SFX_BUS, sfxVolume);
	}
}

using Godot;
using System;

public partial class VolumeSettings : Node
{
	[Export] private HSlider masterSlider;
	[Export] private HSlider musicSlider;
	[Export] private HSlider effectsSlider;

    public override void _Ready()
	{
		masterSlider.ValueChanged += OnMasterVolumeChanged;
		musicSlider.ValueChanged += OnMusicVolumeChanged;
		effectsSlider.ValueChanged += OnEffectsVolumeChanged;

		SetSliderValueToBus(masterSlider, "Master");
		SetSliderValueToBus(musicSlider, "Music");
		SetSliderValueToBus(effectsSlider, "Sound Effects");
	}

	private void OnMasterVolumeChanged(double value)
	{
		SetBusVolume("Master", value);
	}

	private void OnMusicVolumeChanged(double value)
	{
		SetBusVolume("Music", value);
	}

	private void OnEffectsVolumeChanged(double value)
	{
		SetBusVolume("Sound Effects", value);
	}

	public static void SetBusVolume(string name, double value)
	{
		int index = AudioServer.GetBusIndex(name);

		AudioServer.SetBusVolumeLinear(index, (float)value);
	}

	public static void SetSliderValueToBus(HSlider slider, string name)
	{
		int index = AudioServer.GetBusIndex(name);
		slider.Value =  AudioServer.GetBusVolumeLinear(index);
	}
}

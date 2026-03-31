using Godot;
using Godot.NativeInterop;
using System;
using System.ComponentModel;

public partial class TimeUI : Node
{
	[Export] private HSlider _timeSlider;
	[Export] private Label _timeLabel;
	public override void _Process(double delta)
	{
		float time = GameManager.Instance.CurrentTime/GameManager.MAX_TIME * 720 + 8 * 60;
		double hours = Math.Floor(time / 60);
		double minutes = Math.Floor(time % 60);

		_timeSlider.Value = GameManager.Instance.CurrentTime/GameManager.MAX_TIME;
		if(GameManager.Instance.CurrentTime < GameManager.MAX_TIME)
		{
			string timeLabel = "";
			if(hours < 10)
			{
				timeLabel += "0";
			}
			timeLabel += hours + ":";
			if(minutes < 10)
			{
				timeLabel += "0";
			}
			timeLabel += minutes;
			_timeLabel.Text = timeLabel;
		}
		else
		{
			_timeLabel.Text = "Night";
		}
	}


}

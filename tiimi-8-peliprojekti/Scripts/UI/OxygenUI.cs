using Godot;
using System;

public partial class OxygenUI : Control
{
	[Export] private VSlider oxygenSlider;
	[Export] private Label oxygenDeltaLabel;

	private float flashTimer = 0;
	private float flashDuration = 0.1f;

	public override void _Process(double delta)
	{
		if(GameManager.Instance.ActiveAquarium.OxygenDelta > 0)
		{
			oxygenDeltaLabel.Text = "+"+GameManager.Instance.ActiveAquarium.OxygenDelta;
		}
		else
		{
			oxygenDeltaLabel.Text = ""+GameManager.Instance.ActiveAquarium.OxygenDelta;
		}
		oxygenSlider.Value = GameManager.Instance.ActiveAquarium._currentOxygen;
		if (!GameManager.Instance.ActiveAquarium.MinMaxIdealOxygen())
		{
			flashTimer += (float)delta;

			if(flashTimer < flashDuration) return;

			flashTimer = 0;

			if(oxygenSlider.SelfModulate != new Color(1, 1, 1))
			{
				oxygenSlider.SelfModulate = new Color(1, 1, 1);
			}
			else
			{
				oxygenSlider.SelfModulate = new Color(1, 0, 0);
			}
		}
		else if(oxygenSlider.SelfModulate != new Color(1, 1, 1))
		{
			oxygenSlider.SelfModulate = new Color(1, 1, 1);
		}
	}
}

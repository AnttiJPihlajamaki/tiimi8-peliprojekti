using Godot;
using System;

public partial class OxygenUI : Control
{
	[Export] private VSlider oxygenSlider;

	private float flashTimer = 0;
	private float flashDuration = 0.1f;

	public override void _Process(double delta)
	{
		oxygenSlider.Value = GameManager.Instance.ActiveAquarium._currentOxygen;
		if (!GameManager.Instance.ActiveAquarium.MinMaxIdealOxygen())
		{
			flashTimer += (float)delta;

			if(flashTimer < flashDuration) return;

			flashTimer = 0;

			if(SelfModulate != new Color(1, 1, 1))
			{
				SelfModulate = new Color(1, 1, 1);
			}
			else
			{
				SelfModulate = new Color(1, 0, 0);
			}
		}
	}
}

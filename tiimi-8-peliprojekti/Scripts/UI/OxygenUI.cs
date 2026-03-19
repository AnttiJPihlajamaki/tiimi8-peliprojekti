using Godot;
using System;

public partial class OxygenUI : Node
{
	[Export] private VSlider oxygenSlider;

	public override void _Process(double delta)
	{
		oxygenSlider.Value = GameManager.Instance.ActiveAquarium._currentOxygen;
	}
}

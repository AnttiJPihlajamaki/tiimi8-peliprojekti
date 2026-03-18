using Godot;
using System;

public partial class MoneyUI : Node
{
	[Export] private Label moneyLabel;
	[Export] private Label moneyPerSecLabel;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		moneyLabel.Text = ""+Mathf.Floor(GameManager.Instance.Money);
	}
}

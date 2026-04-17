using Godot;
using System;

public partial class MoneyUI : Node
{
	[Export] private Label moneyLabel;
	[Export] private Label moneyPerSecLabel;
	public override void _Ready()
	{
		OnMoneyChanged();
		OnMoneyPerSecondChanged();

		GameManager.Instance.MoneyChanged += OnMoneyChanged;
		GameManager.Instance.MoneyPerSecondChanged += OnMoneyPerSecondChanged;

		/*for(int i = 0; i < 30; i++)
		{
			GD.Print(MoneyConfig.MoneyConversion(Mathf.Pow(10,i)));
		}
		*/
	}

	private void OnMoneyChanged()
	{
		moneyLabel.Text = MoneyConfig.MoneyConversion(GameManager.Instance.Money);
	}

	private void OnMoneyPerSecondChanged()
	{
		moneyPerSecLabel.Text = MoneyConfig.MoneyConversion(GameManager.Instance.MoneyPerSecond);
	}
}

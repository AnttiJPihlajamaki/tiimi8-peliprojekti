using Godot;
using System;

public partial class MoneyUI : Node
{
	[Export] private Label moneyLabel;
	[Export] private Label moneyPerSecLabel;

	private double _timer = 0f;
	private double _timerMax = 1f;
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

    public override void _Process(double delta)
    {
        if(_timer < 1)
		{
			_timer += delta;
		}
		else
		{
			_timer = 0;
			OnMoneyChanged();
		}
    }


	private void OnMoneyChanged()
	{
		moneyLabel.Text = MoneyConfig.MoneyConversion(GameManager.Instance.Money);
	}

	private void OnMoneyPerSecondChanged()
	{
		moneyPerSecLabel.Text = MoneyConfig.MoneyConversion(GameManager.Instance.MoneyPerSecond);
	}

    public override void _ExitTree()
	{
		GameManager.Instance.MoneyChanged -= OnMoneyChanged;
		GameManager.Instance.MoneyPerSecondChanged -= OnMoneyPerSecondChanged;
	}
}

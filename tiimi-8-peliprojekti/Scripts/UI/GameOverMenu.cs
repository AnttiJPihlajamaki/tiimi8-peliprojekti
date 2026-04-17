using Godot;
using Godot.Collections;
using System;

public partial class GameOverMenu : Node
{
	[Export] private Label _totalMoneyAmount;
	[Export] private Label _dayPassedAmount;
	[Export] private Array<CanvasItem> _itemsToHide;
	[Export] private Array<CanvasItem> _itemsToShow;

    public override void _Ready()
    {
		GameManager.Instance.GameOver += OnGameOver;
    }
	public void OnGameOver()
	{
		_totalMoneyAmount.Text = MoneyConfig.MoneyConversion(GameManager.Instance.TotalMoney);
		_dayPassedAmount.Text = ""+GameManager.Instance.DaysPassed;
		if(_itemsToHide.Count > 0)
		{
			foreach(CanvasItem item in _itemsToHide)
			{
				item.Visible = false;
			}
		}
		if(_itemsToShow.Count > 0)
		{
			foreach(CanvasItem item in _itemsToShow)
			{
				item.Visible = true;
			}
		}
	}

    public override void _ExitTree()
    {
		GameManager.Instance.GameOver -= OnGameOver;
    }
}

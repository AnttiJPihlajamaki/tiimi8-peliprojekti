using Godot;
using Godot.Collections;
using System;

public partial class ShopMenu : Control
{
	[Export] private Array<CanvasItem> _itemsToHide;
	[Export] private Array<CanvasItem> _itemsToShow;

	[Export] private Node _tools;
	[Export] private Button _openButton;
	[Export] private Button _closeButton;

    public override void _Ready()
	{
		_openButton.Pressed += OpenMenu;
		_closeButton.Pressed += CloseMenu;
	}

	private void OpenMenu()
	{
		AudioManager.Instance.PlaySound("Pop");
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
		_tools.ProcessMode = ProcessModeEnum.Disabled;
	}
	private void CloseMenu()
	{
		AudioManager.Instance.PlaySound("Pop");
		if(_itemsToHide.Count > 0)
		{
			foreach(CanvasItem item in _itemsToHide)
			{
				item.Visible = true;
			}
		}
		if(_itemsToShow.Count > 0)
		{
			foreach(CanvasItem item in _itemsToShow)
			{
				item.Visible = false;
			}
		}
		_tools.ProcessMode = ProcessModeEnum.Inherit;
	}
}

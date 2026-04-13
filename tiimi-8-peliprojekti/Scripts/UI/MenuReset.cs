using Godot;
using Godot.Collections;
using System;

public partial class MenuReset : Control
{
	[Export] private Array<CanvasItem> _itemsToHide;
	[Export] private Array<CanvasItem> _itemsToShow;
	public override void _Ready()
	{
		VisibilityChanged += Reset;
	}

	private void Reset()
	{
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
}

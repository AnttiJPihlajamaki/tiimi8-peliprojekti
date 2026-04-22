using Godot;
using System;
using Godot.Collections;

public partial class ShowCreditsButton : Button
{
	[Export] private Array<CanvasItem> _itemsToHide;
	[Export] private Array<CanvasItem> _itemsToShow;

	public override void _Ready()
	{
		Pressed += Press;
	}

	protected virtual void Press()
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

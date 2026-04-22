using Godot;
using Godot.Collections;

public partial class SwitchToggle : Button
{
	[Export] private Array<CanvasItem> _itemsToShow;
	[Export] private Array<CanvasItem> _itemsToHide;
	public override void _Ready()
	{
		Toggle(false);
		Toggled += ToggleWithSound;
	}
	protected virtual void ToggleWithSound(bool set)
	{
		if (set) AudioManager.Instance.PlaySound("Pop");
		ToggleItem(set);
	}

	protected virtual void Toggle(bool set)
	{
		ToggleItem(set);
	}
	private void ToggleItem(bool set)
	{
		if(_itemsToShow.Count > 0)
		{
			foreach(CanvasItem item in _itemsToShow)
			{
				if (set)
				{
					item.Visible = true;
				}
				else
				{
					item.Visible = false;
				}
			}
		}
		if(_itemsToShow.Count > 0)
		{
			foreach(CanvasItem item in _itemsToHide)
			{
				if (set)
				{
					item.Visible = false;
				}
				else
				{
					item.Visible = true;
				}
			}
		}
	}
}

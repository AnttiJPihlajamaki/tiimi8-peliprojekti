using Godot;
using Godot.Collections;

public partial class HideToggle : Button
{
	[Export] private Array<CanvasItem> _itemsToToggle;
	public override void _Ready()
	{
		Toggle(false);
		Toggled += Toggle;
	}

	protected virtual void Toggle(bool set)
	{
		ToggleItem(set);
	}
	private void ToggleItem(bool set)
	{
		if(_itemsToToggle.Count > 0)
		{
			foreach(CanvasItem item in _itemsToToggle)
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
	}
}

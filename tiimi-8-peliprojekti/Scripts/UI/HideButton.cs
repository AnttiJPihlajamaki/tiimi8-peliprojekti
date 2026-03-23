using Godot;
using System;

public partial class HideButton : Button
{
	[Export] CanvasItem _hideElement;
	public override void _Ready()
	{
		Toggle(false);
		Toggled += Toggle;
	}

	protected virtual void Toggle(bool set)
	{
		ToggleElement(set);
	}

	private void ToggleElement(bool set)
	{
		if (set)
		{
			_hideElement.Visible = true;
		}
		else
		{
			_hideElement.Visible = false;
		}
	}
}

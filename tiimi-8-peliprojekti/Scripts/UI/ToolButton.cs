using Godot;
using System;
using System.ComponentModel;

public partial class ToolButton : HideToggle
{
	[Export] Node _tool;
	protected override void Toggle(bool set)
	{
		base.Toggle(set);
		SetTool(set);
	}

	private void SetTool(bool set)
	{
		if (set)
		{
			_tool.ProcessMode = ProcessModeEnum.Inherit;
		}
		else
		{
			_tool.ProcessMode = ProcessModeEnum.Disabled;
		}
	}
}

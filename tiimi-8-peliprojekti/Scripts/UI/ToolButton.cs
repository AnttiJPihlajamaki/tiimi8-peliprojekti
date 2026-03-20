using Godot;
using System;
using System.ComponentModel;

public partial class ToolButton : Button
{
	[Export] Tool _tool;
	[Export] Control _toolInfo;

 	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Toggled += SetTool;
	}

	private void SetTool(bool set)
	{
		if (set)
		{
			_tool.ProcessMode = ProcessModeEnum.Inherit;
			_toolInfo.Visible = true;
		}
		else
		{
			_tool.ProcessMode = ProcessModeEnum.Disabled;
			_toolInfo.Visible = false;
		}
	}
}

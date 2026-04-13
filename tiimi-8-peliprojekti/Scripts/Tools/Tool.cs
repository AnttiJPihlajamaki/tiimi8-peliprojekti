using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public abstract partial class Tool : Node
{
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventScreenTouch)
		{
			InputEventScreenTouch screenTouch = @event as InputEventScreenTouch;
			if (screenTouch.IsReleased())
			{
				ToolFunction(screenTouch);
			}
		}
    }
	protected virtual string Info() // ! Temporary method to get info about the tool while UI gets added !
	{
		return ""+GetType();
	}

	protected abstract void ToolFunction(InputEventScreenTouch @event); // Abstract methods for all tool functions
}

using Godot;
using System;

public abstract partial class Tool : Node
{
	[Export] private Aquarium _aquarium;
	public Inventory _inventory;
	public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(InputConfig.ToolFunction))
		{
			ToolFunction();
		}
        if (@event.IsActionPressed(InputConfig.ToolIncrease))
		{
			ToolIncrease();
		}
        if (@event.IsActionPressed(InputConfig.ToolDecrease))
		{
			ToolDecrease();
		}
    }
	public virtual string Info()
	{
		return ""+GetType();
	}

	public abstract void ToolFunction();
	public abstract void ToolIncrease();
	public abstract void ToolDecrease();
}

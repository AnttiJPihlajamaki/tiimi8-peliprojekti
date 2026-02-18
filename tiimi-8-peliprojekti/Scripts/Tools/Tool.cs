using Godot;
using System;

public abstract partial class Tool : Node
{
	[Export] private Aquarium _aquarium;
	[Export] private Shop _shop;

	public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ToolFunction"))
		{
			ToolFunction();
		}
        if (@event.IsActionPressed("ToolIncrease"))
		{
			ToolIncrease();
		}
        if (@event.IsActionPressed("ToolDecrease"))
		{
			ToolDecrease();
		}
    }

	public abstract void ToolFunction();
	public abstract void ToolIncrease();
	public abstract void ToolDecrease();
}

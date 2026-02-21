using Godot;
using System;

public abstract partial class Tool : Node
{
	[Export] private Aquarium _aquarium; // Reference to aquarium the tool is located
	public Inventory _inventory; // Reference to player inventory
	public override void _Input(InputEvent @event) // Input for tools
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
	public virtual string Info() // ! Temporary method to get info about the tool while UI gets added !
	{
		return ""+GetType();
	}

	public abstract void ToolFunction(); // Abstract methods for all tool functions
	public abstract void ToolIncrease(); // (Maybe change to virtual or partial since not all tool use these)
	public abstract void ToolDecrease();
}

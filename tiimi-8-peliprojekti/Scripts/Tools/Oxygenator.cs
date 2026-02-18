using Godot;
using System;

public partial class Oxygenator : Tool
{
	[Export] private Aquarium _aquarium;
	[Export] private Shop _shop;

	private bool _isActive = false;
	private float _basePower = 1.0f;
	private float _power;

    public override void _Ready()
    {
        _power = _basePower;
    }

    public override void _PhysicsProcess(double delta)
    {
		if (_isActive)
		{
        	_aquarium._currentOxygen += _power * (float)delta;
		}
    }

	public override void _Input(InputEvent @event)
    {
        if (@event.IsActionReleased("ToolFunction"))
		{
			ToolFunction();
		}
		base._Input(@event);
    }

	public override void ToolFunction()
	{
		if (_isActive)
		{
			_isActive = false;
		}
		else
		{
			_isActive = true;
		}
	}
	public override void ToolIncrease()
	{
		_power += _basePower;
	}

	public override void ToolDecrease()
	{
		_power -= _basePower;
	}
}

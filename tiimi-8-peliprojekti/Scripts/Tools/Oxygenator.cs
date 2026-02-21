using Godot;
using System;

public partial class Oxygenator : Tool
{
	[Export] private Aquarium _aquarium; // Reference to aquarium
	private bool _isActive = false; // Whether the tool is being used
	private float _maxPower = 10.0f; // Minimum/maximum oxygen that can be removed/added per second
	private float _basePower = 1.0f; // The default power of the tool, also used as the amount of change when changing power
	private float _power; // Current power of the tool

    public override void _EnterTree()
    {
        _power = _basePower; // Set current power to base
    }
    public override void _PhysicsProcess(double delta)
    {
		if (_isActive && _aquarium.MinMaxOxygen()) // While active changes oxygen by current power
		{
        	_aquarium._currentOxygen += _power * (float)delta;
		}
    }

	public override void _Input(InputEvent @event)
    {
        if (@event.IsActionReleased("ToolFunction")) // Added input to make tool inactive when the ToolFunction input is released
		{
			ToolFunction();
		}
		base._Input(@event);
    }

	public override void ToolFunction() // Changes tool from inactive to active and back again
	{
		_isActive = !_isActive;
	}
	public override void ToolIncrease() // Increase power up to max
	{
		if(_power < _maxPower)
		{
			_power += _basePower;
		}
	}

	public override void ToolDecrease() // Decrease power down to min
	{
		if(_power > -_maxPower)
		{
			_power -= _basePower;
		}
	}

	
	public override string Info() // ! Temporary method to get info about the tool while UI gets added !
	{
		return GetType() + " = ( Power: " + _power + " )";
	}
}

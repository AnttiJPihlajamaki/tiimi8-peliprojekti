using Godot;
using System;

public partial class Oxygenator : Tool
{
	[Export] private Aquarium _aquarium;
	private bool _isActive = false;
	private float _maxPower = 10.0f;
	private float _basePower = 1.0f;
	private float _power;

    public override void _EnterTree()
    {
        _power = _basePower;
    }
    public override void _PhysicsProcess(double delta)
    {
		if (_isActive && _aquarium.MinMaxOxygen())
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
		if(_power < _maxPower)
		{
			_power += _basePower;
		}
	}

	public override void ToolDecrease()
	{
		if(_power > -_maxPower)
		{
			_power -= _basePower;
		}
	}

	
	public override string Info()
	{
		return GetType() + " = ( Power: " + _power + " )";
	}
}

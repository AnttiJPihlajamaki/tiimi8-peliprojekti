using Godot;
using System;

public partial class Oxygenator : Tool
{
	private bool _isActive = false; // Whether the tool is being used
	[Export] private float _maxPower = 10.0f; // Minimum/maximum oxygen that can be removed/added per second
	private float _basePower = 0f; // The default power of the tool, also used as the amount of change when changing power
	private float _power; // Current power of the tool

	[Export] private VSlider powerSlider;
    public override void _Ready()
	{
        _power = _basePower;

		powerSlider.MinValue = -_maxPower;
		powerSlider.MaxValue = _maxPower;
		powerSlider.Value = _basePower;

		powerSlider.DragEnded += SetPower;
	}

    public override void _PhysicsProcess(double delta)
    {
		if (_isActive && GameManager.Instance.ActiveAquarium.MinMaxOxygen()) // While active changes oxygen by current power
		{
        	GameManager.Instance.ActiveAquarium._currentOxygen += _power * (float)delta;
		}
    }

	public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventScreenTouch)
		{
			InputEventScreenTouch eventTouch = @event as InputEventScreenTouch;
			if (eventTouch.IsPressed())
			{
				_isActive = true;
			}
			else if(eventTouch.IsReleased())
			{
				_isActive = false;
			}
		}
    }

	protected override void ToolFunction(InputEventScreenTouch @event) // Changes tool from inactive to active and back again
	{
	}
	private void SetPower(bool valueChanged)
	{
		if (valueChanged)
		{
			_power = Mathf.Clamp((float)powerSlider.Value, -_maxPower, _maxPower);
		}
	}

	
	protected override string Info() // ! Temporary method to get info about the tool while UI gets added !
	{
		return GetType() + " = ( Power: " + _power + " )";
	}
}

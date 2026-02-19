using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Aquarium : Node2D
{

	[Export] private float _maxOxygen = 100f;
	[Export] private float _minOxygen = 0;
	[Export] public float _currentOxygen = 100f;
	[Export] private float _oxygenDelta = 0;
	[Export] public Array<Tool> _tools;
	private int _currentTool = 0;
	public List<Fish> _fish = [];
	public List<Food> _food = [];
	[Export] public NavigationRegion2D _navigationRegion;

    public override void _Ready()
    {
        for(int i = 0; i < _tools.Count; i++)
		{
			if(i == _currentTool)
			{
				_tools[i].ProcessMode = ProcessModeEnum.Inherit;
			}
			else
			{
				_tools[i].ProcessMode = ProcessModeEnum.Disabled;
			}
		}
    }

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_up"))
		{
			if(_currentTool < _tools.Count-1)
			{
				ChangeTool(_currentTool + 1);
				GD.Print("Tool Change " + _currentTool);
			}
			else
			{
				ChangeTool(0);
				GD.Print("Tool Change " + _currentTool);
			}
		}
		else if (@event.IsActionPressed("ui_down"))
		{
			if(_currentTool > 0)
			{
				ChangeTool(_currentTool - 1);
				GD.Print("Tool Change " + _currentTool);
			}
			else
			{
				ChangeTool(_tools.Count - 1);
				GD.Print("Tool Change " + _currentTool);
			}

		}
	}

    public override void _PhysicsProcess(double delta)
    {
		if(_currentOxygen >= _minOxygen && _currentOxygen <= _maxOxygen)
		{
			_currentOxygen += _oxygenDelta * (float)delta;

			if(_currentOxygen < _minOxygen)
			{
				_currentOxygen = _minOxygen;
			}
			else if(_currentOxygen > _maxOxygen)
			{
				_currentOxygen = _maxOxygen;
			}
		}

		GD.Print("Oxygen: " + _currentOxygen + " / " + _maxOxygen + " Tool: " + _tools[_currentTool].GetType() + " (" + (_currentTool + 1) + "/" + _tools.Count+")");
    }

	public void UpdateOxygenDelta()
	{
		_oxygenDelta = 0;
		foreach(Fish fish in _fish)
		{
			_oxygenDelta -= fish._oxygenUsage;
		}
	}

	public void ChangeTool(int newTool)
	{
		_tools[_currentTool].ProcessMode = ProcessModeEnum.Disabled;
		_currentTool = newTool;
		_tools[_currentTool].ProcessMode = ProcessModeEnum.Inherit;
	}
}

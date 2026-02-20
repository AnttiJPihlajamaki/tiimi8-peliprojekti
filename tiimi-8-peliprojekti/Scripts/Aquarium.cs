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
	[Export] public Inventory _inventory;
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
			_tools[i]._inventory = _inventory;
		}
    }

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed(InputConfig.NextTool))
		{
			if(_currentTool < _tools.Count-1)
			{
				ChangeTool(_currentTool + 1);
			}
			else
			{
				ChangeTool(0);
			}
		}
		else if (@event.IsActionPressed(InputConfig.PreviousTool))
		{
			if(_currentTool > 0)
			{
				ChangeTool(_currentTool - 1);
			}
			else
			{
				ChangeTool(_tools.Count - 1);
			}

		}
	}

    public override void _PhysicsProcess(double delta)
    {
		_currentOxygen = Mathf.Clamp(_currentOxygen + (_oxygenDelta * (float)delta), _minOxygen, _maxOxygen);

		GD.Print("Money: " + (int)Math.Round(_inventory._money) + " Oxygen: " + (int)Math.Round(_currentOxygen) + " / " + _maxOxygen + " Tool: " + _tools[_currentTool].Info());
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

	public void AddFish(Fish newFish)
	{
		newFish.Name = newFish._name + "#" + newFish.GetInstanceId();

		AddChild(newFish);
		_fish.Add(newFish);

		newFish._aquarium = this;
		newFish._inventory = _inventory;

		newFish._movementTarget = new Marker2D();
		_navigationRegion.AddChild(newFish._movementTarget);
		newFish.SetRandomMarkerPosition();

		UpdateOxygenDelta();
	}
	public void RemoveFish(Fish newFish)
	{
		_fish.Remove(newFish);
		
		UpdateOxygenDelta();
	}

	public void AddFood(Food newFood)
	{
		newFood.Name = newFood._name + "#" + newFood.GetInstanceId();

		AddChild(newFood);
		_food.Add(newFood);

		newFood.removalDistance = _navigationRegion.GetBounds().Size.Y/2;

		newFood._aquarium = this;
	}

	public bool MinMaxOxygen()
	{
		return _currentOxygen >= _minOxygen && _currentOxygen <=_maxOxygen;
	}
}

using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Aquarium : Node2D
{
	[Export] private float _maxOxygen = 100f; // The maximum amount of oxygen in the aquarium
	[Export] private float _minOxygen = 0; // The minimum amount of oxygen in the aquarium
	[Export] public float _currentOxygen = 100f; // The current amount of oxygen in the aquarium
	[Export] private float _oxygenDelta = 0; // The amount of oxygen removed/added per second
	[Export] public Inventory _inventory; // Reference to player inventory
	[Export] public Array<Tool> _tools; // Array of tools the player can use
	private int _currentTool = 0; // The index of the current tool in use
	public List<Fish> _fish = []; // List of fish in the aquarium
	public List<Food> _food = []; // List of food in the aquarium
	[Export] public NavigationRegion2D _navigationRegion; // The navigation region the fish can move in

    public override void _Ready()
    {
        for(int i = 0; i < _tools.Count; i++) // Initializes tools so only current tool is active
		{
			if(i == _currentTool)
			{
				_tools[i].ProcessMode = ProcessModeEnum.Inherit;
			}
			else
			{
				_tools[i].ProcessMode = ProcessModeEnum.Disabled;
			}
			_tools[i]._inventory = _inventory; // Sets inventory reference for tools
		}
    }

	public override void _Input(InputEvent @event) // Handle input for changing tools
	{
		if (@event.IsActionPressed(InputConfig.NextTool))
		{
			if(_currentTool < _tools.Count-1)
			{
				ChangeTool(_currentTool + 1); // Change to next tool
			}
			else
			{
				ChangeTool(0); // Change to first tool if current tool is last tool i the array
			}
		}
		else if (@event.IsActionPressed(InputConfig.PreviousTool))
		{
			if(_currentTool > 0)
			{
				ChangeTool(_currentTool - 1); // Change to previous tool
			}
			else
			{
				ChangeTool(_tools.Count - 1); // Change to last tool if current tool is first tool i the array
			}

		}
	}

    public override void _PhysicsProcess(double delta)
    {
		_currentOxygen = Mathf.Clamp(_currentOxygen + (_oxygenDelta * (float)delta), _minOxygen, _maxOxygen); // Change current oxygen according to the delta

		GD.Print("Money: " + (int)Math.Round(_inventory._money) + " Oxygen: " + (int)Math.Round(_currentOxygen) + " / " + _maxOxygen + " Tool: " + _tools[_currentTool].Info());
		// ! Temporary method to get info while UI gets added !
    }

	public void UpdateOxygenDelta() // Updates change in current oxygen
	{
		_oxygenDelta = 0;
		foreach(Fish fish in _fish)
		{
			_oxygenDelta -= fish._oxygenUsage; // Reduce delta for each fish by their oxygen usage
		}
	}

	public void ChangeTool(int newTool) // The method to change tools
	{
		_tools[_currentTool].ProcessMode = ProcessModeEnum.Disabled; // Disable previous
		_currentTool = newTool; // Set current to next
		_tools[_currentTool].ProcessMode = ProcessModeEnum.Inherit; // Enable next
	}

	public void AddFish(Fish newFish) // The method to handle adding fish to aquarium
	{
		newFish.Name = newFish._name + "#" + newFish.GetInstanceId(); // Gives object unique name

		AddChild(newFish); // Adds fish as child of aquarium
		_fish.Add(newFish); // Adds fish to list of fish

		newFish._aquarium = this; // Adds reference of aquarium to the fish
		newFish._inventory = _inventory; // Adds reference of player inventory to the fish

		newFish._movementTarget = new Marker2D(); // Creates a marker for the fish' AI to follow
		_navigationRegion.AddChild(newFish._movementTarget); // Adds the marker as child of the navigation region
		newFish.SetRandomMarkerPosition(); // Set random position for fish to follow to initialize AI

		UpdateOxygenDelta(); // Update change in current oxygen
	}
	public void RemoveFish(Fish newFish) // The method to handle removing fish from aquarium
	{
		_fish.Remove(newFish); // Remove fish from list of fish

		UpdateOxygenDelta(); // Update change in current oxygen
	}

	public void AddFood(Food newFood) // The method to handle adding food to aquarium
	{
		newFood.Name = newFood._name + "#" + newFood.GetInstanceId(); // Gives object unique name

		AddChild(newFood); // Adds food as child of aquarium
		_food.Add(newFood); // Adds food to list of fish

		newFood.removalDistance = _navigationRegion.GetBounds().Size.Y/2; // Set food's removal distance to half of the vertical length of the navigation region

		newFood._aquarium = this; // Adds reference of aquarium to the food
	}

	public bool MinMaxOxygen() // Helper method to check if current oxygen is with min/max
	{
		return _currentOxygen >= _minOxygen && _currentOxygen <=_maxOxygen;
	}
}

using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

public partial class Aquarium : Node2D
{
	[Export] private float _maxOxygen = 100f; // The maximum amount of oxygen in the aquarium
	[Export] private float _minOxygen = 0; // The minimum amount of oxygen in the aquarium
	[Export] private float _maxIdealOxygen = 75f; // The maximum oxygen at which the fish doesn't take damage from over oxygenation
	[Export] private float _minIdealOxygen = 25f; // The minimum oxygen at which the fish doesn't take damage from under oxygenation
	[Export] public float _currentOxygen = 100f; // The current amount of oxygen in the aquarium
	[Export] private float _oxygenDelta = 0; // The amount of oxygen removed/added per second

	public float OxygenDelta
	{
		get{ return _oxygenDelta; }
	}
	public List<AquariumNPC> _npcs = []; // List of fish in the aquarium
	public List<AquariumObject> _objects = []; // List of fish in the aquarium
	public List<Food> _food = []; // List of food in the aquarium

	[Export] private Control _shop;
	[Export] public NavigationRegion2D _navigationRegion; // The navigation region the fish can move in

	private GridPlacer objectPlacer;
	[Export] public GridPlacer ObjectPlacer
	{
		get{ return objectPlacer; }
		set{ objectPlacer = value; }
	}

    public override void _Ready()
    {
		GameManager.Instance.ActiveAquarium = this;
    }

    public override void _PhysicsProcess(double delta)
    {
		_currentOxygen = Mathf.Clamp(_currentOxygen + (_oxygenDelta * (float)delta), _minOxygen, _maxOxygen); // Change current oxygen according to the delta
    }

	public void UpdateOxygenDelta() // Updates change in current oxygen
	{
		_oxygenDelta = 0;
		foreach(AquariumNPC npc in _npcs)
		{
			_oxygenDelta -= npc._oxygenUsage; // Reduce delta for each fish by their oxygen usage
		}
		foreach(AquariumObject obj in _objects)
		{
			_oxygenDelta -= obj._oxygenUsage; // Reduce delta for each fish by their oxygen usage
		}
	}

	public void AddNPC(AquariumNPC newNPC) // The method to handle adding fish to aquarium
	{
		AddChild(newNPC); // Adds fish as child of aquarium
		_npcs.Add(newNPC); // Adds fish to list of fish

		newNPC._aquarium = this; // Adds reference of aquarium to the fish

		newNPC.SetMarkerRegion(_navigationRegion);

		UpdateOxygenDelta(); // Update change in current oxygen
	}

	public void AddObject(AquariumObject newObject)
	{
		newObject.Name = newObject._name + "#" + newObject.GetInstanceId(); // Gives object unique name

		_objects.Add(newObject); // Adds fish to list of fish

		newObject._aquarium = this; // Adds reference of aquarium to the fish

		UpdateOxygenDelta(); // Update change in current oxygen
	}

	public void RemoveObject(AquariumObject obj)
	{
		_objects.Remove(obj);

		UpdateOxygenDelta();
	}
	public void RemoveFish(AquariumNPC npc) // The method to handle removing fish from aquarium
	{
		_npcs.Remove(npc); // Remove fish from list of fish

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

	public void UpdateShopPrices()
	{
		foreach(Node node in _shop.GetChildren())
		{
			if(node is ShopItem)
			{
				ShopItem shopItem = node as ShopItem;
				shopItem.UpdatePrice();
			}
		}
	}

	public bool MinMaxOxygen() // Helper method to check if current oxygen is with min/max
	{
		return _currentOxygen >= _minOxygen && _currentOxygen <=_maxOxygen;
	}
	public bool MinMaxIdealOxygen() // Helper method to check if current oxygen is with min/max
	{
		return _currentOxygen >= _minIdealOxygen && _currentOxygen <=_maxIdealOxygen;
	}
	public bool MaxIdealOxygen() // Helper method to check if current oxygen is with min/max
	{
		return _currentOxygen <=_maxIdealOxygen;
	}

}

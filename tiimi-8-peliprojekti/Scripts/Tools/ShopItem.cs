using Godot;
using System;
using System.IO.Pipes;

// Class that handles buying a specific type of fish
public partial class ShopItem : Tool // ! Is a Tool temporarily while UI gets added !
{
	[Export] public PackedScene _fish; // The PackedScene of the fish
	[Export] public float _basePrice = 50f; // The base price of the fish
	[Export] public float _currentPrice; // The current price of the fish
	[Export] public bool _freeSample; // Whether or the first purchase is free or not
	[Export] public Aquarium _aquarium; // ! The aquarium the tool is located !

	public override void _Ready()
	{
		UpdatePrice(); // Updates the price
	}
	public override string Info() // ! Temporary method to get info about the tool while UI gets added !
	{
		return GetType() + " = ( Fish Cost: " + (int)Math.Round(_currentPrice)+" )";
	}

	public override void ToolFunction()
	{
		Purchase();
	}
	public override void ToolIncrease()
	{

	}

	public override void ToolDecrease()
	{

	}

	public void UpdatePrice() // Updates the current price of the fish
	{
		if (_freeSample) // Set price to 0 if free sample is enabled
		{
			_currentPrice = 0f;
		}
		else // Else set price to base price
		{
			_currentPrice = _basePrice;;
		}

		foreach(AquariumNPC npc in _aquarium._npcs) // Increases price for each fish in the aquarium
		{
			if(npc is Alien) continue;

			if(_currentPrice == 0f && _freeSample) // Logic to handle free sample
			{
				_currentPrice = _basePrice;
			}
			else
			{
				_currentPrice += _currentPrice * 1.2f; // Increase price exponentially for each subsequent fish
			}
		}

		_currentPrice = (float)Math.Round(_currentPrice); // Rounds price to whole number
	}

	public void Purchase() // Handles purchasing new fish
	{
		if(GameManager.Instance.Money >= _currentPrice)
		{
			GameManager.Instance.Money -= _currentPrice; // Checks if player has enough money and removes it
			AddFish(); // Adds fish to aquarium
		}
	}

	public void AddFish()
	{

		Fish newFish = _fish.Instantiate<Fish>(); // Instantiatses new fish

		_aquarium.AddNPC(newFish); // Calls method from _aquarium to add a new fish

		UpdatePrice(); // Updates price
	}
}

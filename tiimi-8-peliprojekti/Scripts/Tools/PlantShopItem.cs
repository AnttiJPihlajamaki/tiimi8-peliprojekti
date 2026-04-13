using Godot;
using System;
using System.IO.Pipes;

// Class that handles buying a specific type of fish
public partial class PlantShopItem : Control // ! Is a Tool temporarily while UI gets added !
{
	[Export] private PackedScene _fishPackedScene; // The PackedScene of the fish
	[Export] private float _basePrice = 50f; // The base price of the fish
	[Export] private float _currentPrice; // The current price of the fish
	[Export] private bool _freeSample; // Whether or the first purchase is free or not

	[Export] private Button _buyButton;
	[Export] private Label _costLabel;
	[Export] private string _name = "NPC";

	public override void _Ready()
	{
		UpdatePrice(); // Updates the price
		_buyButton.Pressed += Purchase;
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
		foreach(AquariumNPC npc in GameManager.Instance.ActiveAquarium._npcs) // Increases price for each fish in the aquarium
		{
			if(npc is not Fish) continue;
			else if(npc.NpcName == _name)
			{
				if(_currentPrice == 0f && _freeSample) // Logic to handle free sample
				{
					_currentPrice = _basePrice;
				}
				else
				{
					_currentPrice += _currentPrice * 1.2f; // Increase price exponentially for each subsequent fish
				}
			}
		}

		_currentPrice = (float)Math.Round(_currentPrice); // Rounds price to whole number
		_costLabel.Text = ""+_currentPrice;
	}

	public void Purchase() // Handles purchasing new fish
	{
		if(GameManager.Instance.Money >= _currentPrice)
		{
			GameManager.Instance.RemoveMoney(_currentPrice); // Checks if player has enough money and removes it
			AddFish(); // Adds fish to aquarium
		}
	}

	public void AddFish()
	{
		Fish newFish = _fishPackedScene.Instantiate<Fish>(); // Instantiatses new fish

		newFish.Name = _name + "#" + newFish.GetInstanceId(); // Gives object unique name
		newFish.NpcName = _name;
		newFish.GlobalPosition = new Vector2((float)GD.RandRange(-600, 600), -600);

		GameManager.Instance.ActiveAquarium.AddNPC(newFish); // Calls method from _aquarium to add a new fish

		UpdatePrice(); // Updates price
	}
}

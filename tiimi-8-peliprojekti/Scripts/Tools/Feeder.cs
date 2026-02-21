using Godot;
using System;

public partial class Feeder : Tool
{

	[Export] public PackedScene _food; // The PackedScene of the food
	[Export] public float _price = 10f; // The price of the food
	[Export] private Aquarium _aquarium; // Reference to aquarium

	public override void ToolFunction()
	{
		if (_inventory._money > _price) // Check if the player has enough money
		{
			_inventory._money -= _price; // Remove price from player
			Food newFood = _food.Instantiate<Food>(); // Instantiates food object
			_aquarium.AddFood(newFood); // Calls method from aqurium to add the food
			newFood.Position = GetViewport().GetCamera2D().GetLocalMousePosition(); // Moves the food to mouse pointer (Need to change or add mobile input)
		}
	}
	public override string Info() // ! Temporary method to get info about the tool while UI gets added !
	{
		return GetType() + " = ( Food Cost: " + (int)Math.Round(_price)+" )";
	}

	public override void ToolIncrease() // Empty methods since the tool doesn't need them
	{
	}

	public override void ToolDecrease()
	{
	}

}

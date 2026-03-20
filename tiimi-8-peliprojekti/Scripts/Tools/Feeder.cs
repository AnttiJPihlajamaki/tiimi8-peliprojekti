using Godot;
using System;

public partial class Feeder : Tool
{

	[Export] public PackedScene _food; // The PackedScene of the food
	[Export] public float _price = 10f; // The price of the food

	protected override void ToolFunction(InputEventScreenTouch @event)
	{
		Vector2 targetPosition = (GetViewport().GetCamera2D().GlobalPosition - GetViewport().GetVisibleRect().Size/2 + @event.Position)/GetViewport().GetCamera2D().Zoom;
		BuyFood(targetPosition);
	}

	private void BuyFood(Vector2 position)
	{
		if (GameManager.Instance.Money > _price) // Check if the player has enough money
		{
			GameManager.Instance.Money -= _price; // Remove price from player
			Food newFood = _food.Instantiate<Food>(); // Instantiates food object
			GameManager.Instance.ActiveAquarium.AddFood(newFood); // Calls method from aqurium to add the food
			newFood.Position = position; // Moves the food to mouse pointer (Need to change or add mobile input)
		}
	}

	protected override string Info() // ! Temporary method to get info about the tool while UI gets added !
	{
		return GetType() + " = ( Food Cost: " + (int)Math.Round(_price)+" )";
	}
}

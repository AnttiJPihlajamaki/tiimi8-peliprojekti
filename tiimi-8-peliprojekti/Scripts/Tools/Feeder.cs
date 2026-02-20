using Godot;
using System;

public partial class Feeder : Tool
{

	[Export] public PackedScene _food;
	[Export] public float _price = 10f;
	[Export] private Aquarium _aquarium;

	public override void ToolFunction()
	{
		if (_inventory._money > _price)
		{
			_inventory._money -= _price;
			Food newFood = _food.Instantiate<Food>();
			_aquarium.AddFood(newFood);
			newFood.Position = GetViewport().GetCamera2D().GetLocalMousePosition();
		}
	}
	public override string Info()
	{
		return GetType() + " = ( Food Cost: " + (int)Math.Round(_price)+" )";
	}

	public override void ToolIncrease()
	{
	}

	public override void ToolDecrease()
	{
	}

}

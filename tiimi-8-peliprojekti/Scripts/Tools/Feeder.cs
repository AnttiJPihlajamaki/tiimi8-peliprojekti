using Godot;
using System;

public partial class Feeder : Tool
{

	[Export] public PackedScene _food;
	[Export] public float _price = 10f;
	[Export] private Aquarium _aquarium;
	[Export] private Shop _shop;

	public override void ToolFunction()
	{
		if (_shop._money > _price)
		{
			_shop._money -= _price;
			Food newFood = _food.Instantiate<Food>();
			newFood.removalDistance = _aquarium._navigationRegion.GetBounds().Size.Y/2;
			newFood._aquarium =_aquarium;
			_aquarium.AddChild(newFood);
			_aquarium._food.Add(newFood);
			newFood.Position = GetViewport().GetCamera2D().GetLocalMousePosition();
		}
	}
	public override void ToolIncrease()
	{

	}

	public override void ToolDecrease()
	{

	}
}

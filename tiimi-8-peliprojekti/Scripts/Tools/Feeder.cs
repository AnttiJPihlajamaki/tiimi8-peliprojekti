using Godot;
using System;

public partial class Feeder : Node
{

	[Export] public PackedScene _food;
	[Export] public float _price = 10f;
	[Export] private Aquarium _aquarium;
	[Export] private Shop _shop;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("UseTool") && _shop._money > _price)
		{
			_shop._money -= _price;
			Food newFood = _food.Instantiate<Food>();
			_aquarium.AddChild(newFood);
			_aquarium._food.Add(newFood);
			newFood.Position = GetViewport().GetCamera2D().GetLocalMousePosition();
		}
    }
}

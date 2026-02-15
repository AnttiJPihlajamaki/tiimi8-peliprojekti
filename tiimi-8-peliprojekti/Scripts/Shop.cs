using Godot;
using System;
using System.Diagnostics;

public partial class Shop : Node
{
	[Export] public PackedScene _fish;
	[Export] public float _money = 0f;
	[Export] public float _price = 50f;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("BuyFish") && _money > _price)
		{
			_money -= _price;
			/*int i = 1;

			string fishName =
			foreach(Node obj in GetNode("/root/Aquarium").GetChildren())
			{
				if(obj.GetType<Fish>() )
				i++;
			}
			newFish.Name = "Fish"+i;
			*/
			Node newFish = _fish.Instantiate<Node>();
			GetParent().AddChild(newFish);
			_price += _price * 1.2f;
		}
    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GD.Print("Money: " + _money + " Fish Cost: " + _price);
	}
}

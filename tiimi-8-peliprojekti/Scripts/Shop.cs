using Godot;
using System;
using System.Diagnostics;

public partial class Shop : Node
{
	[Export] public PackedScene _fish;
	[Export] public float _money = 0f;
	[Export] public float _basePrice = 50f;
	[Export] public float _currentPrice;
	[Export] public bool _freeSample;
	[Export] public Aquarium _aquarium;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		UpdatePrice();
	}

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("BuyFish") && _money >= _currentPrice)
		{
			Purchase();
		}
    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GD.Print("Money: " + (int)Math.Round(_money) + " Fish Cost: " + (int)Math.Round(_currentPrice));
	}

	public void UpdatePrice()
	{
		if (_freeSample)
		{
			_currentPrice = 0f;
		}
		else
		{
			_currentPrice = _basePrice;;
		}

		foreach(Fish fish in _aquarium._fish)
		{
			if(_currentPrice == 0f && _freeSample)
			{
				_currentPrice = _basePrice;
			}
			else
			{
				_currentPrice += _currentPrice * 1.2f;
			}
		}

		_currentPrice = (float)Math.Round(_currentPrice);
	}

	public void Purchase()
	{
		_money -= _currentPrice;

		int i = 1;
		foreach(Fish fish in _aquarium._fish)
		{
			i++;
		}

		Fish newFish = _fish.Instantiate<Fish>();
		newFish.Name = "Fish " + i;
		_aquarium.AddChild(newFish);
		_aquarium._fish.Add(newFish);
		newFish._aquarium = _aquarium;
		newFish._shop = this;

		newFish._movementTarget = new Marker2D();
		_aquarium._navigationRegion.AddChild(newFish._movementTarget);

		newFish.SetRandomMarkerPosition();

		UpdatePrice();
		_aquarium.UpdateOxygenDelta();
	}
}

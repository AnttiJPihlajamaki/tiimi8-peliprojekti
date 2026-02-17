using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Aquarium : Node2D
{

	[Export] public float _maxOxygen = 100f;
	[Export] public float _minOxygen = 0;
	[Export] public float _currentOxygen = 100f;
	[Export] public float _oxygenDelta = 0;
	public List<Fish> _fish = [];
	public List<Food> _food = [];

	[Export] public NavigationRegion2D _navigationRegion;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
		if(_currentOxygen >= _minOxygen && _currentOxygen <= _maxOxygen)
		{
			_currentOxygen += _oxygenDelta * (float)delta;

			if(_currentOxygen < _minOxygen)
			{
				_currentOxygen = _minOxygen;
			}
			else if(_currentOxygen > _maxOxygen)
			{
				_currentOxygen = _maxOxygen;
			}

			GD.Print("Oxygen: " + _currentOxygen + " / " + _maxOxygen);
		}
    }
}

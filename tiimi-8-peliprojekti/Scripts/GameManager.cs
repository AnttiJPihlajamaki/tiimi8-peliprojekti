using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class GameManager : Node // Store player inventory
{
	public float _maxTime = 120f; // maximum time
	public float _nightTime = 119f; // when night will trigger
	public float _daySpeed = 1f;
	public float _currentTime = 110f; // The current time
	public bool _isNight = false; //
	public List<Alien> _nightAliens = new List<Alien>();
	public Aquarium _aquarium;
	public PackedScene _alienScene;

	public static GameManager Instance
	{
		private set;
		get;
	}
	public GameManager()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else if(Instance != this)
		{
			QueueFree();
			return;
		}
	}
    public override void _Ready()
    {
		_alienScene = GD.Load<PackedScene>("res://Assets/Prefabs/Alien.tscn");
    }

	public override void _Process(double delta)
	{
		if(_isNight == false)
		{
			_currentTime += (float)delta * _daySpeed;
		}

		if(_currentTime >= _nightTime && !_isNight)
		{
			_currentTime = _nightTime;
			NightStart();
		}

		if (_currentTime % 1f < (float)delta) // shows time and if night is true every second
		{
			GD.Print("time: " + (int)_currentTime + " is night: " + _isNight);
		}
	}

	private void NightStart()
	{
		_isNight = true;
		SpawnAliens(3);
	}

	private void SpawnAliens(int amount)
	{
		for (int a = 0; a < amount; a++)
		{
			Alien newAlien = _alienScene.Instantiate<Alien>();
			_aquarium.AddNPC(newAlien);
			_nightAliens.Add(newAlien);
			newAlien.GlobalPosition = GetSpawnPosition();
		}
	}

	private Vector2 GetSpawnPosition()
	{
		Rect2 bounds = _aquarium._navigationRegion.GetBounds();

		int spawnSide = (int)GD.RandRange(0, 4);

		switch (spawnSide)
		{
			case 0: return new Vector2(bounds.Position.X, (float)GD.RandRange(bounds.Position.Y, bounds.End.Y)); // left
			case 1: return new Vector2(bounds.End.X, (float)GD.RandRange(bounds.Position.Y, bounds.End.Y));      // right
			case 2: return new Vector2((float)GD.RandRange(bounds.Position.X, bounds.End.X), bounds.Position.Y); // top
			default: return new Vector2((float)GD.RandRange(bounds.Position.X, bounds.End.X), bounds.End.Y);     // bottom
		}
	}

	#region GameData
	private float _money = 0f; // The amount of money the player has


	public float Money
	{
		get { return _money; }
		set
		{
			_money = value;
		}
	}
	#endregion
}
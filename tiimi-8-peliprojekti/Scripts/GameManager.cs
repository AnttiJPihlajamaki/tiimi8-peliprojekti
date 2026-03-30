using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks.Dataflow;

public partial class GameManager : Node // Store player inventory
{
	private float _maxTime = 60f; // maximum time
	private float _nightTime = 59f; // when night will trigger
	private float _daySpeed = 1f;
	private float _currentTime = 0f; // The current time
	private bool _isNight = false; //
	public List<Alien> _nightAliens = new List<Alien>(); // measure how many aliens are currently in aquarium
	private Aquarium _activeAquarium;
	private PackedScene _alienScene;
	private PackedScene _portalScene;
	private int _difficultyLevel = 3;

	public static GameManager Instance
	{
		private set;
		get;
	}
	public GameManager()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			QueueFree();
			return;
		}
	}

    public override void _EnterTree()
    {
        Reset();
    }
    public override void _Ready()
    {
		Reset();
		_alienScene = GD.Load<PackedScene>("res://Assets/Packed Scenes/Alien.tscn");
		_portalScene = GD.Load<PackedScene>("res://Assets/Packed Scenes/portal.tscn");
    }

	public override void _Process(double delta)
	{
		GD.Print(_currentTime);
		if(ActiveAquarium != null)
		{
			if (_isNight == false)
			{
				_currentTime += (float)delta * _daySpeed;
			}

			if (_currentTime >= _nightTime && !_isNight)
			{
				_currentTime = _nightTime;
				NightStart();
			}
			if(_nightAliens.Count == 0 && _isNight)
			{
				_isNight = false;
				_currentTime = 0f;
				_difficultyLevel++;
			}
		}

		if(_isNight == true && ActiveAquarium._npcs.Count(npc => npc is not Alien) == 0)
		{
			GameOver();
		}
	}

	public void RemoveNightAlien(Alien alien)
	{
		_nightAliens.Remove(alien);
	}

	private void GameOver()
	{
		Reset();
    	GetTree().ChangeSceneToFile("res://Scenes/MainMenuScene.tscn");
	}

	public void Reset()
	{
		_currentTime = 0;
		_money = 0;
		_nightAliens.Clear();
		_difficultyLevel = 3;
	}

	private void NightStart()
	{
		_isNight = true;
		SpawnAliens(_difficultyLevel);
	}

	private void SpawnAliens(int amount)
	{
		for (int a = 0; a < amount; a++)
		{
			Alien newAlien = _alienScene.Instantiate<Alien>();  // new alien added to list _nightAlien
			ActiveAquarium.AddNPC(newAlien);
			_nightAliens.Add(newAlien);
			newAlien.GlobalPosition = GetSpawnPosition();

			Node2D portal = _portalScene.Instantiate<Node2D>();  // portal animation
			ActiveAquarium.AddChild(portal);
			portal.GlobalPosition = newAlien.GlobalPosition;
		}
	}

	private Vector2 GetSpawnPosition()
	{
		Rect2 bounds = ActiveAquarium._navigationRegion.GetBounds();

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
	public Aquarium ActiveAquarium
	{
		get{ return _activeAquarium; }
		set
		{
			_activeAquarium = value;
		}
	}
	#endregion
}
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class GameManager : Node // Store player inventory
{
	private Aquarium _activeAquarium;
	public Aquarium ActiveAquarium
	{
		get{ return _activeAquarium; }
		set
		{
			_activeAquarium = value;
		}
	}
	public const float MAX_TIME = 60f;
	private float _daySpeed = 1f;
	private float _currentTime = 0f; // The current time
	public float CurrentTime
	{
		get { return _currentTime; }
	}
	private bool _isNight = false; //
	public bool IsNight
	{
		get { return _isNight; }
	}
	private List<AquariumNPC> _nightAliens = new List<AquariumNPC>(); // measure how many aliens are currently in aquarium
	private PackedScene _alienScene;
	private PackedScene _snailScene;
	private PackedScene _portalScene;
	private int _difficultyLevel = 3;
	[Signal] public delegate void GameOverEventHandler();

	public int DaysPassed
	{
		get{return _difficultyLevel - 3; }
	}


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
		_alienScene = GD.Load<PackedScene>("res://Assets/Packed Scenes/Alien.tscn");
		_portalScene = GD.Load<PackedScene>("res://Assets/Packed Scenes/portal.tscn");
		_snailScene = GD.Load<PackedScene>("res://Assets/Packed Scenes/Aliensnail.tscn");
    }
	public override void _Process(double delta)
	{
		if (_moneyPerSecond > 0)
		{
			AddMoneyPerSecondToMoney(_moneyPerSecond * (float) delta);
		}
		if(ActiveAquarium != null)
		{
			if (!_isNight)
			{
				_currentTime += (float)delta * _daySpeed;
			}
			if (!_isNight && _currentTime >= MAX_TIME)
			{
				NightStart();
			}
			else if(_isNight && ActiveAquarium._npcs.Count(npc => npc is Fish) == 0)  // GAME OVER CONDITION CHECKIGN FISH
			{
				PauseGame(true);
				EmitSignal(SignalName.GameOver);
			}
			if(_isNight && _nightAliens.Count == 0 )
			{
				AudioManager.Instance.PlayMusic("Aquarium");

				_isNight = false;
				_currentTime = 0f;
				_difficultyLevel = _difficultyLevel + 2;
			}
		}
	}

	public void RemoveNightAlien(AquariumNPC alien)
	{
		_nightAliens.Remove(alien);
	}

	public void PauseGame(bool pause)
	{
		GetTree().Paused = pause;
	}
	public void Reset()
	{
		_isNight = false;
		_currentTime = 0;
		_money = 0;
		_moneyPerSecond = 0;
		_totalMoney = 0;
		_nightAliens.Clear();
		_difficultyLevel = 3;
	}

	private void NightStart()
	{
		_isNight = true;
		SpawnAliens(_difficultyLevel);
		AudioManager.Instance.PlaySound("NIGHTALARM");

		AudioManager.Instance.PlayMusic("Night");
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

		int snailAmount = amount / 3;  // for every 3 aliens, spawn 1 alien snail

		for (int s = 0; s < snailAmount; s++)
		{
			Aliensnail newSnail = _snailScene.Instantiate<Aliensnail>();  // new snail added to list _nightAlien
			ActiveAquarium.AddNPC(newSnail);
			_nightAliens.Add(newSnail);
			newSnail.GlobalPosition = GetSpawnPositionSnail();

			Node2D portal = _portalScene.Instantiate<Node2D>();  // portal animation
			ActiveAquarium.AddChild(portal);
			portal.GlobalPosition = newSnail.GlobalPosition;
		}
	}

	private Vector2 GetSpawnPositionSnail()
	{
		Rect2 bounds = ActiveAquarium._navigationRegion.GetBounds();
		int spawnSide = GD.RandRange(0, 4);
		return new Vector2((float)GD.RandRange(bounds.Position.X, bounds.End.X), bounds.End.Y);
	}

	private Vector2 GetSpawnPosition()
	{
		Rect2 bounds = ActiveAquarium._navigationRegion.GetBounds();

		int spawnSide = GD.RandRange(0, 4);

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
	private float _totalMoney = 0f;
	public float TotalMoney
	{
		get{ return _totalMoney; }
	}
	private float _moneyPerSecond = 0f;

	[Signal] public delegate void MoneyPerSecondChangedEventHandler();
	public float MoneyPerSecond
	{
		get{ return _moneyPerSecond; }
	}
	public void AddMoneyPerSecond(float value)
	{
		_moneyPerSecond += value;
		EmitSignal(SignalName.MoneyPerSecondChanged);
	}
	public void RemoveMoneyPerSecond(float value)
	{
		_moneyPerSecond -= value;
		EmitSignal(SignalName.MoneyPerSecondChanged);
	}

	[Signal] public delegate void MoneyChangedEventHandler();
	public float Money
	{
		get { return _money; }
	}
	public void AddMoney(float value)
	{
		_money += value;
		_totalMoney += value;
		EmitSignal(SignalName.MoneyChanged);
	}
	public void AddMoneyPerSecondToMoney(float value)
	{
		_money += value;
		_totalMoney += value;
	}
	public void RemoveMoney(float value)
	{
		_money -= value;
		EmitSignal(SignalName.MoneyChanged);
	}
	#endregion
}
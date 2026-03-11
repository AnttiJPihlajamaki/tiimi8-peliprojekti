using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class GameManager : Node // Store player inventory
{
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
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Aquarium : Node2D
{
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
}

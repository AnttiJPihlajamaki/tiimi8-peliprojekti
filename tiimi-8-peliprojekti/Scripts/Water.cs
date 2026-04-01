using Godot;
using System;

public partial class Water : Area2D
{
    public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBobyExited;
	}

	private void OnBodyEntered(Node2D body)
	{
		if(body is AquariumNPC)
		{
			AquariumNPC npc = body as AquariumNPC;
			npc.InWater = true;
		}
	}

	private void OnBobyExited(Node2D body)
	{
		if(body is AquariumNPC)
		{
			AquariumNPC npc = body as AquariumNPC;
			npc.InWater = false;
		}
	}
}

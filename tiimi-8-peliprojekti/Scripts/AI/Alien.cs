using Godot;
using System;
using System.ComponentModel;
/*
public partial class Alien : AquariumNPC
{
	[Export] private float attackDamage = 10f;  // The amount of damage the alien does to fish
	[Export] private float attackRange = 30f;  // The range at which the alien can damage fish
	private AquariumNPC nearestFish = null;
	private float nearestDistance = float.MaxValue;
	public override void _Ready()
	{
		_oxygenUsage = 0;  // Alien doesn't use oxygen
		base._Ready();
	}



	public override void _Process(double delta)
	{
	_hunger = _maxHunger; // Alien doesn't become hungry

	nearestFish = null;
	nearestDistance = float.MaxValue;

	foreach (AquariumNPC npc in _aquarium._npcs)  // LOOP FOR FINDING FISH
	{
		if (npc == this) continue;  // skip self and other aliens
		if (npc is Alien) continue;

		// Get distance to current npc
		float dist = GlobalPosition.DistanceTo(npc.GlobalPosition);

		// Calculate if current NPC is closer than the last
		if (dist < nearestDistance)
		{
			nearestDistance = dist;   // update nearest distance
			nearestFish = npc;        // update nearest fish
		}
	}

	if (nearestFish != null)
		{
			// Move the alien to nearest fish
			SetMarkerPosition(nearestFish.GlobalPosition);
		}

	if(newVelocity.X < 0 && !_sprite.FlipH) // Simple if-statement to flip the sprite towards the direction the sprite is moving
	{
		_sprite.FlipH = true;
	}
	else if(newVelocity.X > 0 && _sprite.FlipH)
	{
		_sprite.FlipH = false;
	}

	base._PhysicsProcess(delta);
	}
}
*/

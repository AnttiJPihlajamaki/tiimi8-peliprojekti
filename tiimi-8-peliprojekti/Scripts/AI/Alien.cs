using Godot;
using System;
using System.ComponentModel;

public partial class Alien : AquariumNPC
{
	[Export] private float _attackRange = 80f;
	[Export] private float _attackDamage = 10f;
	[Export] private float _attackSpeed = 1f;
	private float attackCooldown = 0f;
	private AquariumNPC nearestFish = null;
	private float nearestDistance = float.MaxValue;

    public override void _Ready()
	{
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
        nearestFish = null;
        nearestDistance = float.MaxValue;
		attackCooldown += (float)delta;

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

			if (nearestDistance <= _attackRange && attackCooldown >= _attackSpeed)
			{
				AttackTarget();
				attackCooldown = 0f;
			}
		}

	base._PhysicsProcess(delta);
	}

	private void AttackTarget()
	{
		GD.Print("Attacking fish! Distance: " + nearestDistance + " Range: " + _attackRange);
		nearestFish.ChangeHealth(-_attackDamage);
	}
}
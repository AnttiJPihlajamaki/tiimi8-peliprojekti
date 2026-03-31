using Godot;
using System;
using System.ComponentModel;
using System.Linq;

public partial class Alien : AquariumNPC
{
	[Export] private float _attackRange = 100f;
	[Export] private float _attackDamage = 10f;
	[Export] private float _attackSpeed = 1f;
	private float attackCooldown = 0f;
    public override void _Ready()
	{
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		if(attackCooldown < _attackSpeed)
		{
			attackCooldown += (float)delta;
		}

		if(GameManager.Instance.ActiveAquarium._npcs.Count(npc => npc is Fish) > 0)
		{
			AquariumNPC nearestFish = null;
			float nearestFishDistance = float.MaxValue;

			foreach (AquariumNPC npc in _aquarium._npcs)  // LOOP FOR FINDING FISH
			{
				if (npc is Fish)
				{
					if (GlobalPosition.DistanceTo(npc.GlobalPosition) < nearestFishDistance)
					{
						nearestFish = npc;	// update nearest fish
						nearestFishDistance = GlobalPosition.DistanceTo(npc.GlobalPosition);   // update nearest distance
					}
				}
			}

			if (nearestFish != null)
			{
				SetMarkerPosition(nearestFish.GlobalPosition);
				if (attackCooldown >= _attackSpeed && GlobalPosition.DistanceTo(nearestFish.GlobalPosition) < _attackRange)
				{
					AttackTarget(nearestFish);
					attackCooldown = 0f;
				}
			}
		}

		base._PhysicsProcess(delta);
	}

	private void AttackTarget(AquariumNPC npc)
	{
		npc.TakeDamage(_attackDamage);
	}

    protected override void Die()
    {
		GameManager.Instance.RemoveNightAlien(this);
        base.Die();
	}
}
using Godot;
using System;

public partial class Aliensnail : AquariumNPC
{
	[Export] private float _attackRange = 100f;
	[Export] private float _attackDamage = 34f;
	[Export] private float _attackSpeed = 1f;
	private float attackCooldown = 0f;

	public override void _Ready()
	{
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
if (attackCooldown < _attackSpeed)
{
    attackCooldown += (float)delta;
}

if (_aquarium._objects.Count > 0)
	{
		AquariumObject nearestPlant = null;
		float nearestPlantDistance = float.MaxValue;

		foreach (AquariumObject targetobject in _aquarium._objects)  // LOOP FOR FINDING PLANTS
		{
			float distance = GlobalPosition.DistanceTo(targetobject.GlobalPosition);
			if (distance < nearestPlantDistance)
			{
				nearestPlant = targetobject;
				nearestPlantDistance = distance;
			}
		}

		if (nearestPlant != null)
		{
			SetMarkerPosition(nearestPlant.GlobalPosition);
			if (attackCooldown >= _attackSpeed && nearestPlantDistance < _attackRange)
			{
				nearestPlant.ChangeHealth(-_attackDamage);
				attackCooldown = 0f;
			}
		}
	}

		base._PhysicsProcess(delta);
	}

	protected override void Die()
    {
		GameManager.Instance.RemoveNightAlien(this);
        base.Die();
	}
}
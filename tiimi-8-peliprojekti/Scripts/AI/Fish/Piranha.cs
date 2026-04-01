using Godot;
using System;
using System.Linq;

public partial class Piranha : Fish
{
	[Export] private float _attackRange = 150f;
	[Export] private float _attackDamage = 15f;
	[Export] private float _attackSpeed = 1f;
	private float attackCooldown = 0f;
	private Marker2D _movementTarget;

    public override void _Ready()
	{
		base._Ready();
	}

	private void AttackTarget(AquariumNPC npc)
	{
		npc.TakeDamage(_attackDamage);
	}

	protected override void Navigation(double delta)
	{
		if(_hunger < _hungryLimit)
		{
			if(GameManager.Instance.ActiveAquarium._npcs.Count(npc => npc is Fish) > 0)
			{
				AquariumNPC nearestFish = null;
				float nearestFishDistance = float.MaxValue;

				foreach (AquariumNPC npc in _aquarium._npcs)  // LOOP FOR FINDING FISH
				{
					if (npc is Fish && npc is not Piranha)
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
						if (nearestFish._health <= 0)
						{
							Nourish(80f);
						}
					}
				}
			}
		}
		if (_navigationAgent.IsNavigationFinished())
		{
			SetRandomMarkerPosition(); // Set a random point to move to if fish is stationary
		}

		SetMovementTarget(); // calculate path

		Vector2 newVelocity = _navigationAgent.Velocity.MoveToward((_navigationAgent.GetNextPathPosition() - GlobalPosition).Normalized() * _speed, (float)delta * _speed); // Calculate movement
		// Uses MoveToward to slow down movement when turning towards new point

		if(newVelocity.X < 0 && _paperdoll.Scale.X != 1) // Simple if-statement to flip the sprite towards the direction the fish is moving
        {
            _paperdoll.ApplyScale(new Vector2(-1 ,1));
		}
		else if(newVelocity.X > 0 && _paperdoll.Scale.X != -1)
		{
            _paperdoll.ApplyScale(new Vector2(-1 ,1));
		}

		if (_navigationAgent.AvoidanceEnabled) // Sets velocity when Avoidance is enabled
        {
            _navigationAgent.Velocity = newVelocity;
        }
        else
        {
            OnVelocityComputed(newVelocity); // Sets velocity when Avoidance is disabled
        }
	}

	private void OnVelocityComputed(Vector2 safeVelocity)
    {
        Velocity = safeVelocity;
        MoveAndSlide();
    }

	public override void _Process(double delta)
	{
		GD.Print(_hunger);
		if(attackCooldown < _attackSpeed)
		{
			attackCooldown += (float)delta;
		}
		base._Process(delta);
	}
}

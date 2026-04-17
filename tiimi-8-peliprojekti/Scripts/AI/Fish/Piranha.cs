using Godot;
using Godot.Collections;
using System;
using System.Linq;

[Tool] public partial class Piranha : Fish
{
	private Color _color1;
	[Export] private Color Color1
	{
		get{ return _color1; }
		set
		{
			_color1 = value;
			ChangeColor(_color1,_color2);
		}
	}
	private Color _color2;
	[Export] private Color Color2
	{
		get{ return _color2; }
		set
		{
			_color2 = value;
			ChangeColor(_color1,_color2);
		}
	}
	[Export] Color _hungryColor1;
	[Export] Color _hungryColor2;
	[Export] private Array<Node2D> _color1Parts;
	[Export] private Array<Node2D> _color2Parts;
	[Export] private Node2D _base;
	protected override void ChangeHunger(float change) // Helper method to change hunger while keeping it within min/max
	{
		base.ChangeHunger(change);
		if(_hunger <= 0 && _base.SelfModulate == _color1)
		{
			ChangeColor(_hungryColor1, _hungryColor2);
		}
		else if(_hunger > 0 && _base.SelfModulate != _color1)
		{
			ChangeColor(_color1, _color2);
		}
	}
	public void ChangeColor(Color color1, Color color2)
	{
		if(_color1Parts != null)
		{
			if(_color1Parts.Count > 0)
			{
				foreach (Node2D part in _color1Parts)
				{
					if(part != null) part.SelfModulate = color1;
				}
			}
		}
		if(_color2Parts != null)
		{
			if(_color2Parts.Count > 0)
			{
				foreach (Node2D part in _color2Parts)
				{
					if(part != null) part.SelfModulate = color2;
				}
			}
		}
	}
	[Export] private float _attackRange = 150f;
	[Export] private float _attackDamage = 10f;
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
		if (GameManager.Instance.ActiveAquarium._npcs.Count(npc => npc is Alien || npc is Aliensnail) > 0)  // PRIORITISE HUNTING ALIENS
		{
			AquariumNPC nearestEnemy = null;
			float nearestEnemyDistance = float.MaxValue;

			foreach (AquariumNPC npc in _aquarium._npcs)  // LOOP FOR FINDING FISH
			{
				if (npc is Alien || npc is Aliensnail)
				{
					if (GlobalPosition.DistanceTo(npc.GlobalPosition) < nearestEnemyDistance)
					{
						nearestEnemy = npc;	// update nearest enemy
						nearestEnemyDistance = GlobalPosition.DistanceTo(npc.GlobalPosition);   // update nearest distance
					}
				}
			}

			if (nearestEnemy != null)
			{
				SetMarkerPosition(nearestEnemy.GlobalPosition);
				if (attackCooldown >= _attackSpeed && GlobalPosition.DistanceTo(nearestEnemy.GlobalPosition) < _attackRange)
				{
					AttackTarget(nearestEnemy);
					attackCooldown = 0f;
					if (nearestEnemy._health <= 0)
					{
						Nourish(100f);
					}
				}
			}
		}

		else if(_hunger < _hungryLimit)  // IF NO ALIENS AND BELOW HUNGERLIMIT HUNT FISH INSTEAD
		{
			if(GameManager.Instance.ActiveAquarium._npcs.Count(npc => npc is Fish) > 0)
			{
				AquariumNPC nearestFish = null;
				float nearestFishDistance = float.MaxValue;

				foreach (AquariumNPC npc in _aquarium._npcs)  // LOOP FOR FINDING FISH
				{
					if (npc is Fish && npc is not Piranha)  // make sure piranha doesn't target itself or other pirhanas
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
		if(attackCooldown < _attackSpeed)
		{
			attackCooldown += (float)delta;
		}
		base._Process(delta);
	}
}

using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Godot;

public partial class Fish : CharacterBody2D
{
	[Export] public float _minSpeed = 100.0f;
	[Export] public float _maxSpeed = 150.0f;
	public float _speed;

	[Export] public Marker2D _movementTarget;
	[Export] private NavigationAgent2D _navigationAgent;
	[Export] private AnimatedSprite2D _sprite;
	[Export] private float _maxHealth = 100.0f;
	[Export] private float _health = 100.0f;
	[Export] private float _maxHunger = 100.0f;
	[Export] private float _hunger = 100.0f;
	[Export] private float _hungryLimit = 50.0f;
	[Export] private float _eatingRange = 25.0f;
	[Export] public float _oxygenUsage = 1.0f;
	[Export] private float _maxOxygen = 75f;
	[Export] private float _minOxygen = 25f;

	public Aquarium _aquarium;
 	public Shop _shop;
	private float moneyPerSecond = 50f;
    public override void _EnterTree()
	{
	}


	public override void _Ready()
	{
		_navigationAgent.VelocityComputed += OnVelocityComputed;
		_navigationAgent.MaxSpeed = _maxSpeed;
	}
	public override void _Process(double delta)
	{
		_shop._money += moneyPerSecond * (float)delta;
	}

	private void SetMovementTarget()
	{
		_navigationAgent.TargetPosition = _movementTarget.Position;
	}
	public void SetMarkerPosition(Vector2 position)
	{
		_movementTarget.Position = position;

		_speed = _maxSpeed;
	}
	public void SetRandomMarkerPosition()
	{
		float boundsWidth = _aquarium._navigationRegion.GetBounds().Size.X;
		float boundsHeight = _aquarium._navigationRegion.GetBounds().Size.Y;

		_movementTarget.Position = new Vector2((float)GD.RandRange(-boundsWidth/2,boundsWidth/2),(float)GD.RandRange(-boundsHeight/2,boundsHeight/2));

		_speed = (float)GD.RandRange(_minSpeed, _maxSpeed);
	}
	public override void _PhysicsProcess(double delta)
	{
		ProcessHealth(delta);
		ProcessHunger(delta);

		if (_navigationAgent.IsNavigationFinished())
		{
			SetRandomMarkerPosition();
		}
		if(_hunger < _hungryLimit)
		{
			SetMarkerPositionFood();
		}

		SetMovementTarget();

		Vector2 newVelocity = (_navigationAgent.GetNextPathPosition() - GlobalPosition).Normalized() * _speed;
		newVelocity += Vector2.Down * 25f;

		if(newVelocity.X < 0)
        {
            _sprite.FlipH = true;
		}
		else
		{
            _sprite.FlipH = false;
		}

		 if (_navigationAgent.AvoidanceEnabled)
        {
            _navigationAgent.Velocity = newVelocity;
        }
        else
        {
            OnVelocityComputed(newVelocity);
        }

		_navigationAgent.Velocity = newVelocity * _speed * (float)delta;
	}
	private void OnVelocityComputed(Vector2 safeVelocity)
    {
        Velocity = safeVelocity;
        MoveAndSlide();
    }

	private void SetMarkerPositionFood()
	{
		if(_aquarium._food.Count > 0)
		{
			float shortestDistance = GlobalPosition.DistanceTo(_aquarium._food[0].GlobalPosition);
			Food nearestFood = _aquarium._food[0];
			foreach(Food food in _aquarium._food)
			{
				if(shortestDistance > GlobalPosition.DistanceTo(food.GlobalPosition))
				{
					shortestDistance = GlobalPosition.DistanceTo(food.GlobalPosition);
					nearestFood = food;
				}
			}

			SetMarkerPosition(nearestFood.GlobalPosition);

			if (GlobalPosition.DistanceTo(nearestFood.GlobalPosition) <= _eatingRange)
			{
				_aquarium._food.Remove(nearestFood);
				_hunger += nearestFood._nourishment;
				nearestFood.Destroy();
			}
		}
	}

	private void ProcessHunger(double delta)
	{
		if(_hunger > 0)
		{
			_hunger -= (float)delta;
		}
		else
		{
			if(_hunger < 0)
			{
				_hunger = 0;
			}
			_health -= (float)delta;
		}

	}
	private void ProcessHealth(double delta)
	{
		//Oxygen
		if(_aquarium._currentOxygen > _maxOxygen || _aquarium._currentOxygen < _minOxygen)
		{
			_health -= (float)delta;
		}
		else if(_health < _maxHealth)
		{
			_health += (float)delta;
		}
		//Health
		if(_health > _maxHealth)
		{
			_health = _maxHealth;
		}
		else if(_health <= 0f)
		{
			Die();
		}
	}

	private void Die()
	{
		_aquarium._fish.Remove(this);
		_aquarium.UpdateOxygenDelta();
		_shop.UpdatePrice();
		QueueFree();
	}
}

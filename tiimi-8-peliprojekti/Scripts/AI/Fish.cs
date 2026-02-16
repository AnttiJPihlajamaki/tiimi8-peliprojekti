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

	[Export] public float _maxHunger = 100.0f;
	[Export] public float _hunger = 100.0f;
	[Export] public float _hungryLimit = 50.0f;
	[Export] public float eatingRange = 25.0f;

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
		if(_hunger <= 0)
		{
			_aquarium._fish.Remove(this);
			_shop.UpdatePrice();
			QueueFree();
		}
		else
		{
			_hunger -= (float)delta;
		}
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
		float boundsWidth = _movementTarget.GetParent<NavigationRegion2D>().GetBounds().Size.X;
		float boundsHeight = _movementTarget.GetParent<NavigationRegion2D>().GetBounds().Size.Y;

		_movementTarget.Position = new Vector2((float)GD.RandRange(-boundsWidth/2,boundsWidth/2),(float)GD.RandRange(-boundsHeight/2,boundsHeight/2));

		_speed = (float)GD.RandRange(_minSpeed, _maxSpeed);
	}
	public override void _PhysicsProcess(double delta)
	{
		if (_navigationAgent.IsNavigationFinished())
		{
			SetRandomMarkerPosition();
		}
		if(_hunger < _hungryLimit)
		{
			FindFood();
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

	private void FindFood()
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

			if (GlobalPosition.DistanceTo(nearestFood.GlobalPosition) <= eatingRange)
			{
				_aquarium._food.Remove(nearestFood);
				_hunger += nearestFood._nourishment;
				nearestFood.Eaten();
			}
		}
	}
}

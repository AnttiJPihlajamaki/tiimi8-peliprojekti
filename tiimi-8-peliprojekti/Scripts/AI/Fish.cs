using System;
using Godot;

public partial class Fish : CharacterBody2D
{
	[Export] public float _minSpeed = 100.0f;
	[Export] public float _maxSpeed = 150.0f;
	public float _speed;

	[Export] private Marker2D _movementTarget;
	[Export] private NavigationAgent2D _navigationAgent;
	[Export] private AnimatedSprite2D _sprite;

	[Export] public float _hunger = 100.0f;
	[Export] public float _hungryLimit = 50.0f;

	
	[Export] private Shop _shop;
	private float moneyPerSecond = 50f;
    public override void _EnterTree()
	{
		_movementTarget = new Marker2D();
		GetNode("/root/Aquarium/NavigationRegion2D").AddChild(_movementTarget);
	}


	public override void _Ready()
	{
		_navigationAgent.VelocityComputed += OnVelocityComputed;

		_navigationAgent.MaxSpeed = _maxSpeed;
		_shop = GetNode<Shop>("/root/Aquarium/Shop");

		SetMarkerPosition();
	}
	public override void _Process(double delta)
	{
		_shop._money += moneyPerSecond * (float)delta;
		_hunger -= (float)delta;
	}

	private void SetMovementTarget()
	{
		_navigationAgent.TargetPosition = _movementTarget.Position;
	}

	private void SetMarkerPosition()
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
			SetMarkerPosition();
		}

		SetMovementTarget();

		Vector2 newVelocity = (_navigationAgent.GetNextPathPosition() - this.GlobalPosition).Normalized() * _speed;

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
}

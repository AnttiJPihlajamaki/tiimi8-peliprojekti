using System;
using Godot;

public partial class Food : CharacterBody2D
{
	 [Export]
	private float _speed = 50;
	 [Export]
	public float _nourishment = 20;

	public float removalDistance;
	public Aquarium _aquarium;

	public override void _PhysicsProcess(double delta)
	{
        Velocity = Vector2.Down * _speed;

		if(GlobalPosition.Y >= removalDistance)
		{
			Destroy();
		}

        MoveAndSlide();
	}

	public void Destroy()
	{
		_aquarium._food.Remove(this);
		QueueFree();
	}
}

using System;
using Godot;

public partial class Food : CharacterBody2D
{
	 [Export]
	private float _speed = 50;
	 [Export]
	public float _nourishment = 20;

	public override void _PhysicsProcess(double delta)
	{
        Velocity = Vector2.Down * _speed;

        MoveAndSlide();
	}
}

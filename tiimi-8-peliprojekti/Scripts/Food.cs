using System;
using Godot;

public partial class Food : CharacterBody2D
{
	 [Export]
	public float _speed = 50;
	
	public override void _PhysicsProcess(double delta)
	{
        Velocity = Vector2.Down * _speed;

        MoveAndSlide();
	}
}

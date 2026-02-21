using System;
using Godot;

public partial class Food : CharacterBody2D
{
	
	[Export] public string _name = "Food"; // The name of type of food
	[Export] private float _speed = 50; // The speed at which the food falls down
	[Export] public float _nourishment = 20; // The amount of hunger the food restores
	

	public float removalDistance; // The distance at which the food gets removed
	public Aquarium _aquarium; // Reference to aquarium

	public override void _PhysicsProcess(double delta)
	{
        Velocity = Vector2.Down * _speed; // Set velocity to make the food fall (Maybe change to Rigidbody2D?)

		if(GlobalPosition.Y >= removalDistance) // Check whether food is below removal distance
		{
			Destroy(); // Removes food
		}

        MoveAndSlide();
	}

	public void Destroy() // Handles removing food
	{
		_aquarium._food.Remove(this); // remove reference to his food from aquarium
		QueueFree(); // delete the node
	}
}

using Godot;
using Godot.Collections;

// Parent class for all different types of fish
public partial class AquariumObject : Node2D
{
	[Export] public string _name = "NPC"; // The name of type of fish
	[Export] protected Node2D _paperdoll;	// Sprite of the fish
	[Export] private float _maxHealth = 100.0f;	// Maximum health
	[Export] private float _health = 100.0f; // Current health
	[Export] public float _oxygenUsage = -1.0f; // The amount of oxygen the fish uses
	[Export] private float _oxygenDamage = 1.0f; // The amount of damage the NPC takes from unideal oxygen per second
	private Array<GridCell> objectCells;
	public void SetObjectCells(Array<GridCell> newCells)
	{
		objectCells = newCells;
	}

	public Aquarium _aquarium;
	public override void _PhysicsProcess(double delta)
	{
		ProcessOxygen(delta); // for processing health

	}

	private void ProcessOxygen(double delta)
	{
		if(!_aquarium.MaxIdealOxygen()) // If the aquarium's oxygen is outside of ideal range fish takes damage over time
		{
			ChangeHealth(-(float)delta * _oxygenDamage); // Reduces health over time
		}
	}
	private void ProcessRegen(double delta)
	{
		//Health
		if(_health < _maxHealth && _aquarium.MinMaxIdealOxygen()) // If the aquarium's oxygen is in ideal range fish heals over time
		{
			ChangeHealth((float)delta); // Increases health over time
		}
	}

	public void ChangeHealth(float change) // Helper method to change health while keeping it within min/max
	{
		_health = Mathf.Clamp(_health + change , 0 , _maxHealth);
		if(_health <= 0) // If health changes to 0 the fish dies
		{
			Die();
		}
	}

	private void Die() // Method that handles the fish dying
	{
		_aquarium.RemoveObject(this); // Removes fish from aquarium
		if(objectCells != null)
		{
			foreach(GridCell cell in objectCells)
			{
				cell.Full = false;
			}
		}
		QueueFree(); // Removes the node
	}
}

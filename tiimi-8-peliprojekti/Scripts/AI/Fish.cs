using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Godot;

// Parent class for all different types of fish
public partial class Fish : CharacterBody2D
{
	[Export] public string _name = "Guppy"; // The name of type of fish
	[Export] public float _minSpeed = 100.0f; // Minimum movement speed
	[Export] public float _maxSpeed = 150.0f; // Maximum movement speed
	public float _speed; // Current speed the fish is moving at

	[Export] public Marker2D _movementTarget;	// The target on the navigation region the fish is trying to move to
	[Export] private NavigationAgent2D _navigationAgent;	// The navigation agent component to handle AI on the navigation region
	[Export] private AnimatedSprite2D _sprite;	// Sprite of the fish
	[Export] private float _maxHealth = 100.0f;	// Maximum health
	[Export] private float _health = 100.0f; // Current health
	[Export] private float _maxHunger = 100.0f; // Maximum hunger the fish can have
	[Export] private float _hunger = 100.0f; // Current hunget the fish has
	[Export] private float _hungryLimit = 50.0f; // The point of hunger at which the fish starts searching for food
	[Export] private float _eatingRange = 25.0f; // The range at which a fish can eat food
	[Export] public float _oxygenUsage = 1.0f; // The amount of oxygen the fish uses
	[Export] private float _maxOxygen = 75f; // The maximum oxygen at which the fish doesn't take damage from over oxygenation
	[Export] private float _minOxygen = 25f; // The minimum oxygen at which the fish doesn't take damage from under oxygenation

	public Aquarium _aquarium; // Reference to the aquarium component in which the fish is located
 	public Inventory _inventory; // Reference to players inventory
	private float moneyPerSecond = 50f; // The amount of money the fish generates per second

	public override void _Ready()
	{
		_navigationAgent.VelocityComputed += OnVelocityComputed;
	}
	private void SetMovementTarget() // Set navigation target for agent and calculate path
	{
		_navigationAgent.TargetPosition = _movementTarget.Position;	
	}
	public void SetMarkerPosition(Vector2 position) // Sets a new position for the marker set speed to maximum
	{
		_movementTarget.Position = position;
		_speed = _maxSpeed;
	}
	public void SetRandomMarkerPosition() // Calculates a random point on the navigation region for the marker
	{
		float boundsWidth = _aquarium._navigationRegion.GetBounds().Size.X; // Calculates the navigation regions horizontal bounds
		float boundsHeight = _aquarium._navigationRegion.GetBounds().Size.Y; // Calculates the navigation regions vertical bounds

		_movementTarget.Position = new Vector2((float)GD.RandRange(-boundsWidth/2,boundsWidth/2),(float)GD.RandRange(-boundsHeight/2,boundsHeight/2)); // Sets the marker position to a random point on the navigation region

		_speed = (float)GD.RandRange(_minSpeed, _maxSpeed);	// Sets random speed within min/max speed
	}
	public override void _PhysicsProcess(double delta)
	{
		_inventory._money += moneyPerSecond * (float)delta; // Adds money per second
		ProcessHealth(delta); // for processing health
		ProcessHunger(delta); // for processing hunger

		if (_navigationAgent.IsNavigationFinished())
		{
			SetRandomMarkerPosition(); // Set a random point to move to if fish is stationary
		}
		if(_hunger < _hungryLimit)
		{
			SetMarkerPositionFood(); // Set marker on nearest food if hunger is too low
		}

		SetMovementTarget(); // calculate path

		Vector2 newVelocity = _navigationAgent.Velocity.MoveToward((_navigationAgent.GetNextPathPosition() - GlobalPosition).Normalized() * _speed, (float)delta * _speed); // Calculate movement
		// Uses MoveToward to slow down movement when turning towards new point

		if(newVelocity.X < 0 && !_sprite.FlipH) // Simple if-statement to flip the sprite towards the direction the fish is moving
        {
            _sprite.FlipH = true;
		}
		else if(_sprite.FlipH)
		{
            _sprite.FlipH = false;
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

	private void SetMarkerPositionFood() // Sets marker on nearest food
	{
		if(_aquarium._food.Count > 0) // If-statement to check whether there is any food in the aquarium
		{
			Food nearestFood = _aquarium._food[0]; // Set the nearest food to be the first one in the list and calculates the distance to it
			float nearestDistance = GlobalPosition.DistanceTo(_aquarium._food[0].GlobalPosition);

			foreach(Food food in _aquarium._food) // Checks through all the food in aquarium to see if another food is closer to the fish
			{
				if(nearestDistance > GlobalPosition.DistanceTo(food.GlobalPosition))
				{
					nearestDistance = GlobalPosition.DistanceTo(food.GlobalPosition);
					nearestFood = food;
				}
			}

			SetMarkerPosition(nearestFood.GlobalPosition); // Sets marker on nearest food

			if (GlobalPosition.DistanceTo(nearestFood.GlobalPosition) <= _eatingRange) // Checks if the food is in range for the fish to eat it
			{
				_hunger = Mathf.Clamp(_hunger + nearestFood._nourishment , 0 , _maxHunger); // Eats the food if it's in range
				nearestFood.Destroy();
			}
		}
	}

	private void ProcessHunger(double delta) 
	{
		ChangeHunger(-(float)delta); // Reduces hunger meter over time
		if(_hunger <= 0) // While hunger is at 0 the fish takes damage over time instead
		{
			ChangeHealth(-(float)delta); // Reduces health over time
		}

	}
	private void ProcessHealth(double delta)
	{
		//Oxygen
		if(!_aquarium.MinMaxOxygen()) // If the aquarium's oxygen is outside of ideal range fish takes damage over time
		{
			ChangeHealth(-(float)delta); // Reduces health over time
		}
		//Health
		else if(_health < _maxHealth && _hunger > 0) // If the aquarium's oxygen is in ideal range fish heals over time
		{
			ChangeHealth((float)delta); // Increases health over time
		}
	}

	private void ChangeHunger(float change) // Helper method to change hunger while keeping it within min/max
	{
		_hunger = Mathf.Clamp(_hunger + change , 0 , _maxHunger);
	}

	private void ChangeHealth(float change) // Helper method to change health while keeping it within min/max
	{
		_health = Mathf.Clamp(_health + change , 0 , _maxHealth);
		if(_health <= 0) // If health changes to 0 the fish dies
		{
			Die();
		}
	}

	private void Die() // Method that handles the fish dying
	{
		_aquarium.RemoveFish(this); // Removes fish from aquarium
		QueueFree(); // Removes the node

		// ! Temporary Price Update !
		ShopItem shopItem;
		foreach(Tool tool in _aquarium._tools)
		{
			if (tool is ShopItem)
			{
				shopItem = tool as ShopItem;
				shopItem.UpdatePrice();
			}
		}
	}
}

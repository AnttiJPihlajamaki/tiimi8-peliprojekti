using System.Collections;
using Godot;

// Parent class for all different types of fish
public partial class AquariumNPC : CharacterBody2D
{
	[Export] private string _name = "NPC"; // The name of type of fish

	public string NpcName
	{
		get { return _name;}
		set { _name = value;}
	}
	[Export] protected float _minSpeed = 100.0f; // Minimum movement speed
	[Export] protected float _maxSpeed = 150.0f; // Maximum movement speed
	protected float _speed; // Current speed the fish is moving at
	private Marker2D _movementTarget;	// The target on the navigation region the fish is trying to move to
	[Export] protected NavigationAgent2D _navigationAgent;	// The navigation agent component to handle AI on the navigation region
	[Export] protected Node2D _paperdoll;	// Sprite of the fish
	[Export] private float _maxHealth = 100.0f;	// Maximum health
	[Export] public float _health = 100.0f; // Current health
	[Export] public float _oxygenUsage = 1.0f; // The amount of oxygen the fish uses
	[Export] private float _oxygenDamage = 1.0f; // The amount of damage the NPC takes from unideal oxygen per second
	[Export] protected float _maxHunger = 100.0f; // Maximum hunger the fish can have
	[Export] protected float _hunger = 100.0f; // Current hunger the fish has
	[Export] private float _hungryLimit = 50.0f; // The point of hunger at which the fish starts searching for food
	[Export] private float _hungerDamage = 1.0f; // The amount of damage the NPC takes from hunger per second
	[Export] private float _eatingRange = 25.0f; // The range at which a fish can eat food

	public Aquarium _aquarium; // Reference to the aquarium component in which the fish is located

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
		ProcessOxygen(delta); // for processing health
		ProcessHunger(delta); // for processing hunger
		ProcessRegen(delta); // for regenning health

		Navigation(delta);
	}
	private void OnVelocityComputed(Vector2 safeVelocity)
    {
        Velocity = safeVelocity;
        MoveAndSlide();
    }

	protected virtual void Navigation(double delta)
	{
		if(_hunger < _hungryLimit)
		{
			SetMarkerPositionFood(); // Set marker on nearest food if hunger is too low
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

	protected void SetMarkerPositionFood() // Sets marker on nearest food
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
				nearestFood.Eat(this);
			}
		}
	}

	protected void ProcessOxygen(double delta)
	{
		if(!_aquarium.MinMaxIdealOxygen()) // If the aquarium's oxygen is outside of ideal range fish takes damage over time
		{
			ChangeHealth(-(float)delta * _oxygenDamage); // Reduces health over time
		}
	}
	protected void ProcessHunger(double delta)
	{
		ChangeHunger(-(float)delta); // Reduces hunger meter over time
		if(_hunger <= 0) // While hunger is at 0 the fish takes damage over time instead
		{
			ChangeHealth(-(float)delta * _hungerDamage); // Reduces health over time
		}
	}
	protected void ProcessRegen(double delta)
	{
		//Health
		if(_health < _maxHealth && _aquarium.MinMaxIdealOxygen() && _hunger > 0) // If the aquarium's oxygen is in ideal range fish heals over time
		{
			ChangeHealth((float)delta); // Increases health over time
		}
	}

	protected virtual void ChangeHunger(float change) // Helper method to change hunger while keeping it within min/max
	{
		_hunger = Mathf.Clamp(_hunger + change , 0 , _maxHunger);
	}
	public void Nourish(float amount)
	{
		ChangeHunger(amount);
	}

	protected void ChangeHealth(float change) // Helper method to change health while keeping it within min/max
	{
		_health = Mathf.Clamp(_health + change , 0 , _maxHealth);
		if(_health <= 0) // If health changes to 0 the fish dies
		{
			Die();
		}
	}
	public void TakeDamage(float amount)
	{
		FlashRed();
		ChangeHealth(-amount);
	}

	private void FlashRed()
	{
		Tween tween = CreateTween();
		tween.TweenProperty(_paperdoll, "modulate", new Color(1, 0, 0), 0.05f); // flash red
		tween.TweenProperty(_paperdoll, "modulate", new Color(1, 1, 1), 0.05f); // color back to normal
	}

	protected virtual void Die() // Method that handles the fish dying
	{
		_aquarium.RemoveFish(this); // Removes fish from aquarium
		QueueFree(); // Removes the node
	}

	public void SetMarkerRegion(NavigationRegion2D navigationRegion)
	{
		_movementTarget = new Marker2D(); // Creates a marker for the fish' AI to follow
		navigationRegion.AddChild(_movementTarget); // Adds the marker as child of the navigation region
		SetRandomMarkerPosition(); // Set random position for fish to follow to initialize AI
	}
}

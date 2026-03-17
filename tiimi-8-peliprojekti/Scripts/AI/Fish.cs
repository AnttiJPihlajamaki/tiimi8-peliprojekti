using Godot;

// Parent class for all different types of fish
public partial class Fish : AquariumNPC
{
	[Export] private float moneyPerSecond = 50f; // The amount of money the fish generates per second
	public override void _PhysicsProcess(double delta)
	{
		GameManager.Instance.Money += moneyPerSecond * (float)delta; // Adds money per second

		base._PhysicsProcess(delta);
	}
}

using Godot;
using Godot.Collections;

// Parent class for all different types of fish
public partial class Fish : AquariumNPC
{
	[Export] private float moneyPerSecond = 50f; // The amount of money the fish generates per second

    public override void _Ready()
    {

		if(GameManager.Instance != null)
        {
            GameManager.Instance.AddMoneyPerSecond(moneyPerSecond); // Adds money per second
            base._Ready();
        }
    }


    protected override void Die()
    {
		GameManager.Instance.RemoveMoneyPerSecond(moneyPerSecond);
        base.Die();
		GameManager.Instance.ActiveAquarium.UpdateShopPrices();
    }


}

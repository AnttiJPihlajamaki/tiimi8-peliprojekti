using Godot;
using Godot.Collections;

// Parent class for all different types of fish
public partial class Fish : AquariumNPC
{
	[Export] private float _moneyPerSecond = 50f; // The amount of money the fish generates per second
    private bool _dead = false;

    public override void _Ready()
    {

		if(GameManager.Instance != null)
        {
            GameManager.Instance.AddMoneyPerSecond(_moneyPerSecond); // Adds money per second
            base._Ready();
            if (IsPickable())
            {
                InputEvent += OnFishTapped;
            }
        }
    }


    protected override void Die()
    {
        if(!_dead){
            _dead = true;
            GameManager.Instance.RemoveMoneyPerSecond(_moneyPerSecond);
            base.Die();
            GameManager.Instance.ActiveAquarium.UpdateShopPrices();

			AudioManager.Instance.PlaySound("Fish Splat");
        }
    }

	private void OnFishTapped(Node viewport, InputEvent @event, long shape_idx)
	{
		if (@event is InputEventScreenTouch)
		{
            AudioManager.Instance.PlaySound("Coin", -0.5f);
			GameManager.Instance.AddMoney(_moneyPerSecond);
		}
	}


}

using Godot;
using System;
using System.IO.Pipes;

// Class that handles buying a specific type of fish
public partial class PlantShopItem : Control // ! Is a Tool temporarily while UI gets added !
{
	[Export] private PackedScene _plantPackedScene; // The PackedScene of the fish
	[Export] private float _price = 50f; // The base price of the fish

	[Export] private Button _buyButton;
	[Export] private Label _costLabel;
	[Export] private string _name = "NPC";

	public override void _Ready()
	{
		_buyButton.Pressed += OpenObjectPlacer;
		_costLabel.Text = ""+_price;
	}

	public void OpenObjectPlacer() // Handles purchasing new fish
	{
		GameManager.Instance.ActiveAquarium.ObjectPlacer.SetObject(_plantPackedScene, _price);
	}
}

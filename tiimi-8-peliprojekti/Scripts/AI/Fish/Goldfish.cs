using System.ComponentModel;
using Godot;
using Godot.Collections;

public partial class Goldfish : Fish
{
	[Export] Color _bodyColor;
	[Export] Color _finColor;
	[Export] Color _hungryBodyColor;
	[Export] Color _hungryFinColor;
	[Export] private Array<Node2D> _fins;
	[Export] private Node2D _body;
	protected override void ChangeHunger(float change) // Helper method to change hunger while keeping it within min/max
	{
		_hunger = Mathf.Clamp(_hunger + change , 0 , _maxHunger);
		if(_hunger <= 0 && _body.Modulate == _bodyColor)
		{
			ChangeColor(_hungryBodyColor, _hungryFinColor);
		}
		else if(_hunger > 0 && _body.Modulate != _bodyColor)
		{
			ChangeColor(_bodyColor, _finColor);
		}
	}
	public void ChangeColor(Color bodyColor, Color finColor)
	{
		_body.Modulate = bodyColor;

		foreach (Node2D fin in _fins)
		{
			fin.Modulate = finColor;
		}
	}
}

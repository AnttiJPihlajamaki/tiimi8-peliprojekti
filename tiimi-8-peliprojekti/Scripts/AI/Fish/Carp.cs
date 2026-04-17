using Godot;
using Godot.Collections;
using System;

[Tool] public partial class Carp : Fish
{
    private Color _color1;
	[Export] private Color Color1
	{
		get{ return _color1; }
		set
		{
			_color1 = value;
			ChangeColor(_color1,_color2);
		}
	}
	private Color _color2;
	[Export] private Color Color2
	{
		get{ return _color2; }
		set
		{
			_color2 = value;
			ChangeColor(_color1,_color2);
		}
	}
	[Export] Color _hungryColor1;
	[Export] Color _hungryColor2;
	[Export] private Array<Node2D> _color1Parts;
	[Export] private Array<Node2D> _color2Parts;
	[Export] private Array<Node2D> _inBetweenParts;
	[Export] private Node2D _base;
    public override void _Ready()
    {

		if(GameManager.Instance != null)
        {
			ChangeColor(_color1,_color2);
			base._Ready();
        }
    }
	protected override void ChangeHunger(float change) // Helper method to change hunger while keeping it within min/max
	{
		base.ChangeHunger(change);
		if(_hunger <= 0 && _base.SelfModulate == _color1)
		{
			ChangeColor(_hungryColor1, _hungryColor2);
		}
		else if(_hunger > 0 && _base.SelfModulate != _color1)
		{
			ChangeColor(_color1, _color2);
		}
	}
	public void ChangeColor(Color color1, Color color2)
	{
		if(_color1Parts != null)
		{
			if(_color1Parts.Count > 0)
			{
				foreach (Node2D part in _color1Parts)
				{
					if(part != null) part.SelfModulate = color1;
                    foreach (Node2D child in part.GetChildren())
                    {
                        if(part != null) child.SelfModulate = color2;
                    }
				}
			}
		}
		if(_color2Parts != null)
		{
			if(_color2Parts.Count > 0)
			{
				foreach (Node2D part in _color2Parts)
				{
					if(part != null) part.SelfModulate = color2;
				}
			}
		}
		if(_inBetweenParts != null)
		{
			if(_inBetweenParts.Count > 0)
			{
				foreach (Node2D part in _inBetweenParts)
				{
					if(part != null) part.SelfModulate = color1.Lerp(color2,.5f);
				}
			}
		}
	}
}

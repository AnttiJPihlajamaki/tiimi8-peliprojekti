using Godot;
using System;

public partial class GridObject : StaticBody2D
{
	[Export] private Vector2 size = new Vector2(1,1);
	[Export] private Vector2 offset = new Vector2(0,0);

	private Rect2 GetRect()
	{
		Vector2 objectPosition = new(GlobalPosition.X - size.X/2, GlobalPosition.Y - size.Y/2);

		return new Rect2(objectPosition, size);
	}
}

using Godot;
using System;

[Tool] public partial class GridCell : Area2D
{
	[Export] private MeshInstance2D _meshInstance2d;
	[Export] private CollisionShape2D _collisionShape2d;

	private Vector2 cellSize = new (1,1);

    public override void _Ready()
    {
        SetCellSize(cellSize);

		Grid grid = GetParent<Grid>();
		ChangeColor(grid.DefaultColor);
    }

	private void ChangeColor(Color newColor)
	{
		_meshInstance2d.SelfModulate = newColor;
	}

	private Rect2 GetRect()
	{
		return new Rect2(new Vector2(GlobalPosition.X, GlobalPosition.Y), cellSize);
	}

	public void SetCellSize(Vector2 newSize)
	{
		cellSize = newSize;

        PlaneMesh planeMeshInstance2D =_meshInstance2d.Mesh as PlaneMesh;
		RectangleShape2D rectangleShape2D = _collisionShape2d.Shape as RectangleShape2D;

		planeMeshInstance2D.Size = cellSize;
		rectangleShape2D.Size = cellSize;
	}

}

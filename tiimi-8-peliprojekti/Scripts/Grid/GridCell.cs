using Godot;
using System;

[Tool] public partial class GridCell : Area2D
{
	[Export] private MeshInstance2D _meshInstance2d;
	[Export] private CollisionShape2D _collisionShape2d;

	private Vector2 cellSize = new (1,1);

	private bool full = false;
	public bool Full
	{
		get{ return full; }
		set{ full = value; }
	}

    public override void _Ready()
    {
		if (IsPickable())
		{
			InputEvent += GetGridPosition;
		}

        SetCellSize(cellSize);

		Grid grid = GetParent<Grid>();
		ChangeColor(grid.DefaultColor);
    }

	public void ChangeColor(Color newColor)
	{
		_meshInstance2d.SelfModulate = newColor;
	}

	public Rect2 GetRect()
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

	private void GetGridPosition(Node viewport, InputEvent @event, long shape_idx)
	{
		if (@event is InputEventScreenDrag)
		{
			GameManager.Instance.ActiveAquarium.ObjectPlacer.TargetPosition = GlobalPosition;
		}
	}

}

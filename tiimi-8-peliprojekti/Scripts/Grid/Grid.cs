using Godot;
using System;
[Tool] public partial class Grid : Node2D
{
	private int _gridWidth = 5;
	[Export] private int GridWidth
	{
		get{ return _gridWidth; }
		set
		{
			_gridWidth = value;
			RecreateGrid();
		}
	}
	private int _gridHeight = 5;
	[Export] private int GridHeight
	{
		get{ return _gridHeight; }
		set
		{
			_gridHeight = value;
			RecreateGrid();
		}
	}
	private Vector2 cellSize = new Vector2(1,1);
	[Export] private Vector2 CellSize
	{
		get{ return cellSize; }
		set
		{
			cellSize = value;
			RecreateGrid();
		}
	}
	[Export] private Color _defaultColor;
	public Color DefaultColor
	{
		get{ return _defaultColor; }
	}
	private PackedScene _gridCell = GD.Load("res://Assets/Packed Scenes/grid_cell.tscn") as PackedScene;

    public override void _Ready()
	{
		RecreateGrid();
	}


	private void RemoveGrid()
	{
		if(_gridWidth <= 0) return;
		if(_gridHeight <= 0) return;
		if(GetChildCount() < 1) return;

		foreach(Node node in GetChildren())
		{
			node.QueueFree();
		}
	}

	private void RecreateGrid()
	{
		RemoveGrid();
		CreateGrid();
		SetPosition();
	}
	private void CreateGrid()
	{
		if(_gridWidth <= 0) return;
		if(_gridHeight <= 0) return;

		if(_gridCell != null)
		{
			for(int height = 0; height < _gridHeight; height++)
			{
				for(int width = 0; width < _gridWidth; width++)
				{
					GridCell gridCell = _gridCell.Instantiate<GridCell>();

					gridCell.SetCellSize(cellSize);
					Vector2 offset = new(width * cellSize.X, height * cellSize.Y);
					gridCell.GlobalPosition += offset;

					AddChild(gridCell);

				}
			}
		}
	}

	private void SetPosition()
	{
		Position = new(-(_gridWidth * cellSize.X / 2 - cellSize.X / 2), -(_gridHeight * cellSize.Y / 2 - cellSize.Y / 2));
	}

}

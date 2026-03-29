using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class GridPlacer : Node2D
{
	[Export] private Grid _grid;
	[Export] private PackedScene _seaweed;

	[Export] private Color _invalidColor;
	[Export] private Color _validColor;
	[Export] private Color _fullColor;
	

	private GridObject _gridObject;
	[Export] private bool _isValid = false;
	private Array<GridCell> _objectCells;
	private Vector2 targetPosition;
	public Vector2 TargetPosition
	{
		get{ return targetPosition; }
		set
		{
			targetPosition = value;
			MovePlacement();
		}
	}

    public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventScreenTouch)
		{
			InputEventScreenTouch screenTouch = @event as InputEventScreenTouch;
			if (screenTouch.IsPressed() && _gridObject == null)
			{
				CreatePlacement();
			}
			else if(screenTouch.IsReleased() && _isValid)
			{
				PlacePlacement(_objectCells);
			}
		}
	}

	private void CreatePlacement()
	{
		GridObject newPlacement = _seaweed.Instantiate<GridObject>();
		newPlacement.AdjustOffset(_grid.CellSize/2);
		AddChild(newPlacement);
		_gridObject = newPlacement;
	}

	private void MovePlacement()
	{
		if(_gridObject != null)
		{
			_gridObject.GlobalPosition = targetPosition + _grid.CellSize/2;

			ResetHighlight();
			_objectCells = GetObjectCells();
			_isValid = CheckAndHighlightCells(_objectCells);
			_gridObject.SetCanvasOrder(Mathf.RoundToInt((targetPosition.Y- GlobalPosition.Y) / _grid.CellSize.Y));
		}
	}
	private void PlacePlacement(Array<GridCell> objectCells)
	{
		_gridObject = null;
		_isValid = false;

		foreach(GridCell cell in objectCells)
		{
			cell.Full = true;
		}

		ResetHighlight();
	}
	private void ResetHighlight()
	{
		foreach(GridCell cell in _grid.GetChildren())
		{
			cell.ChangeColor(_grid.DefaultColor);
		}
	}
	private Array<GridCell> GetObjectCells()
	{
		Array<GridCell> cells = [];

		foreach(GridCell child in _grid.GetChildren())
		{
			if (child.GetRect().Intersects(_gridObject.GetRect()))
			{
				cells.Add(child);
			}
		}

		return cells;
	}

	private bool CheckAndHighlightCells(Array<GridCell> objectCells)
	{
		bool isValid = true;

		float objectCellCount = _gridObject.GetRect().Size.X * _gridObject.GetRect().Size.Y;

		if(objectCellCount != objectCells.Count)
		{
			isValid = false;
		}

		foreach (GridCell cell in _grid.GetChildren())
		{
			if (cell.Full)
			{
				cell.ChangeColor(_fullColor);
			}
		}

		foreach (GridCell cell in objectCells)
		{
			if (cell.Full)
			{
				isValid = false;
				cell.ChangeColor(_invalidColor);
			}
			else
			{
				cell.ChangeColor(_validColor);
			}
		}

		return isValid;
	}

}

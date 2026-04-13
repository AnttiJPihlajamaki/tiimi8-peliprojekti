using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class GridPlacer : Control
{
	[Export] private Grid _grid;
	[Export] private PackedScene _object;

	[Export] private Color _invalidColor;
	[Export] private Color _validColor;
	[Export] private Color _fullColor;
	[Export] private Color _emptyColor;

	[Export] private Node2D _objectParent;

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
			if(_gridObject == null)
			{
				CreatePlacement();
			}
		}
	}

	[Export] private Button buyButton;
	[Export] private Button exitButton;

	[Export] private Array<CanvasItem> _itemsToHide;

	private float _cost = 0f;

    public override void _Ready()
	{
		buyButton.Pressed += BuyPlacement;
		exitButton.Pressed += Exit;
	}

	public void SetObject(PackedScene newObject, float newCost)
	{
		_object = newObject;
		_cost = newCost;

		ProcessMode = ProcessModeEnum.Inherit;
		Visible = true;

		if(_itemsToHide.Count > 0)
		{
			foreach(CanvasItem item in _itemsToHide)
			{
				item.Visible = false;
			}
		}
	}

	private void Exit()
	{
		_object = null;
		_cost = 0;

		ProcessMode = ProcessModeEnum.Disabled;
		Visible = false;
	}
	private void CreatePlacement()
	{
		GridObject newPlacement = _object.Instantiate<GridObject>();

		newPlacement.Name = newPlacement.Name + "#" + newPlacement.GetInstanceId(); // Gives object unique name

		newPlacement.AdjustOffset(_grid.CellSize/2);
		_objectParent.AddChild(newPlacement);
		_gridObject = newPlacement;

		foreach(Node child in _gridObject.GetChildren())
		{
			if(child is AquariumObject)
			{
				AquariumObject obj = child as AquariumObject;
				GameManager.Instance.ActiveAquarium.AddObject(obj);
			}
		}
	}

	private void BuyPlacement()
	{
		PlacePlacement(_objectCells);
	}

	private void MovePlacement()
	{
		if(_gridObject != null)
		{
			_gridObject.GlobalPosition = targetPosition + _grid.CellSize/2;

			ResetHighlight();
			_objectCells = GetObjectCells();
			_isValid = CheckAndHighlightCells(_objectCells);
			_gridObject.SetCanvasOrder(Mathf.RoundToInt((targetPosition.Y- _grid.GlobalPosition.Y) / _grid.CellSize.Y));
		}
	}
	private void PlacePlacement(Array<GridCell> objectCells)
	{
		if(GameManager.Instance.Money >= _cost)
		{
			GameManager.Instance.RemoveMoney(_cost);

			_gridObject = null;
			_isValid = false;

			foreach(GridCell cell in objectCells)
			{
				cell.Full = true;
			}

			ResetHighlight();
		}
		else
		{
			foreach(Node child in _gridObject.GetChildren())
			{
				if(child is AquariumObject)
				{
					AquariumObject obj = child as AquariumObject;
					GameManager.Instance.ActiveAquarium.RemoveObject(obj);
				}
			}

			_gridObject.QueueFree();

			_gridObject = null;
			_isValid = false;

			ResetHighlight();
		}
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
			else
			{
				cell.ChangeColor(_emptyColor);
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

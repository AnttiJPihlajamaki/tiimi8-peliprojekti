using Godot;
using Godot.Collections;

public partial class GridPlacer : Node2D
{
	[Export] private Grid _grid;
	[Export] private PackedScene _seaweed;

	private GridObject _gridObject;
	private bool _isValid = false;
	private Array<GridCell> gridCells;

	private Vector2 targetPosition;
    public override void _Ready()
	{
		foreach(GridCell cell in _grid.GetChildren())
		{
			if (cell.InputPickable)
			{
				cell.InputEvent += GetGridPosition;
			}
		}
	}


	public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventScreenTouch)
		{
			InputEventScreenTouch touchEvent = @event as InputEventScreenTouch;
			if (@event.IsPressed())
			{
				Array<PackedScene> plants = new Array<PackedScene>{_seaweed};
				GridObject newObject = plants.PickRandom().Instantiate<GridObject>();
				AddChild(newObject);
				_gridObject = newObject;
			}
		}
    }
	private void GetGridPosition(Node viewport, InputEvent @event, long shape_idx)
	{
		if (@event is InputEventScreenTouch)
		{
			InputEventScreenTouch touchEvent = @event as InputEventScreenTouch;
			if (@event.IsPressed())
			{
				Array<PackedScene> plants = new Array<PackedScene>{_seaweed};
				GridObject newObject = plants.PickRandom().Instantiate<GridObject>();
				AddChild(newObject);
				_gridObject = newObject;

				newObject.GlobalPosition = GlobalPosition;
			}
		}
	}

}

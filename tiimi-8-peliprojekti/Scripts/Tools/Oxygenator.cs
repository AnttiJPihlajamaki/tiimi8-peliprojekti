using Godot;
using System;

public partial class Oxygenator : Tool
{
	private bool _isActive = false; // Whether the tool is being used
	[Export] private float _maxPower = 10.0f; // Minimum/maximum oxygen that can be removed/added per second
	[Export] private AudioStreamPlayer2D _bubbleSound;
	private float _basePower = 0f; // The default power of the tool, also used as the amount of change when changing power
	private float _power = 0f; // Current power of the tool

	[Export] GpuParticles2D _particles;

	[Export] private VSlider powerSlider;
    public override void _Ready()
	{
        _power = _basePower;

		powerSlider.MinValue = -_maxPower;
		powerSlider.MaxValue = _maxPower;
		powerSlider.Value = _basePower;

		powerSlider.Value = _power;
		SetPower(true);
		powerSlider.DragEnded += SetPower;
	}

    public override void _PhysicsProcess(double delta)
    {
		if (_isActive && GameManager.Instance.ActiveAquarium.MinMaxOxygen()) // While active changes oxygen by current power
		{
        	GameManager.Instance.ActiveAquarium._currentOxygen += _power * (float)delta;
		}
    }

	public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventScreenTouch)
		{
			InputEventScreenTouch screenTouch = @event as InputEventScreenTouch;
			ToolFunction(screenTouch);
		}

		if (@event is InputEventScreenDrag)
		{
			InputEventScreenDrag screenDrag = @event as InputEventScreenDrag;
			_particles.GlobalPosition = (GetViewport().GetCamera2D().GlobalPosition - GetViewport().GetVisibleRect().Size/2 + screenDrag.Position)/GetViewport().GetCamera2D().Zoom;
		}
    }

	protected override void ToolFunction(InputEventScreenTouch screenTouch) // Changes tool from inactive to active and back again
	{
		if (screenTouch.IsPressed())
			{
				_bubbleSound.Play();
				_isActive = true;
				_particles.Emitting = true;
				_particles.GlobalPosition = (GetViewport().GetCamera2D().GlobalPosition - GetViewport().GetVisibleRect().Size/2 + screenTouch.Position)/GetViewport().GetCamera2D().Zoom;
			}
			else if(screenTouch.IsReleased())
			{
				_bubbleSound.Stop();
				_isActive = false;
				_particles.Emitting = false;
			}
	}
	private void SetPower(bool valueChanged)
	{
		if (valueChanged)
		{
			_power = Mathf.Clamp((float)powerSlider.Value, -_maxPower, _maxPower);
			_particles.AmountRatio = Mathf.Abs(_power) / _maxPower;
		}
	}
}

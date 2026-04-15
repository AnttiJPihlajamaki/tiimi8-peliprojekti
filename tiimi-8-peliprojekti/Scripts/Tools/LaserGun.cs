using Godot;
using System;
using System.Linq;

public partial class LaserGun : Tool
{
private PackedScene _laserScene;
[Export] private AudioStreamPlayer2D _laserSound;
[Export] private float _attackDamage = 50f;
[Export] private float _attackRange = 150f;
[Export] private float _attackCooldown = 0.8f;
private float _cooldownTimer = 0f;
	protected override void ToolFunction(InputEventScreenTouch @event)
	{
		if (_cooldownTimer > 0f) return;

		_cooldownTimer = _attackCooldown;

		Vector2 targetPosition = (GetViewport().GetCamera2D().GlobalPosition - GetViewport().GetVisibleRect().Size/2 + @event.Position)/GetViewport().GetCamera2D().Zoom;
		ShootLaser(targetPosition);
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventScreenTouch)
		{
			InputEventScreenTouch screenTouch = @event as InputEventScreenTouch;
			if (screenTouch.IsPressed())
			{
				ToolFunction(screenTouch);
			}
		}
    }

	private void ShootLaser(Vector2 position)
	{
		Node2D laser = _laserScene.Instantiate<Node2D>();  // laser shooting animation
		GameManager.Instance.ActiveAquarium.AddChild(laser);
		laser.GlobalPosition = position;
		laser.ZIndex = 10;
		//_laserSound.Play();
		AudioManager.Instance.PlaySound("Laser");

		foreach (AquariumNPC npc in GameManager.Instance.ActiveAquarium._npcs)  // alien detection and damage
		{
			if (npc is not Alien && npc is not Aliensnail) continue;

			float attackDistance = position.DistanceTo(npc.GlobalPosition);
			if (attackDistance <= _attackRange)
			{
				npc.TakeDamage(_attackDamage);
			}
		}
	}

	public override void _Ready()
	{
		_laserScene = GD.Load<PackedScene>("res://Assets/Packed Scenes/laser.tscn");
	}


	public override void _Process(double delta)
	{
        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= (float)delta;
        }
	}
}

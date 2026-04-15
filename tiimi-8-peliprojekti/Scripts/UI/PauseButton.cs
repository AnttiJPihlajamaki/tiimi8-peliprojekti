using System.Runtime.CompilerServices;
using Godot;
using Godot.Collections;

public partial class PauseButton : HidePress
{
	[Export] private bool _pause;

	protected override void Press()
	{
		base.Press();

		GameManager.Instance.PauseGame(_pause);
	}
}

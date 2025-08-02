using Godot;
using System;
using GMTKJam2025.UI;

public partial class EndSceneBehaviour : Node
{
	public override void _Process(double delta)
	{
		base._Process(delta);
		bool input = false;

		if (Input.IsActionJustPressed("ui_accept")) input = true;
		if (Input.IsActionJustPressed("ui_cancel")) input = true;

		if (input)
		{
			SceneSwitcher.Instance.GoToScene(SceneSwitcher.Instance.Library.Title);
		}
	}
}

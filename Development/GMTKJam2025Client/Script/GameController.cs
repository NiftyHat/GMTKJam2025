using Godot;
using System;
using GMTKJam2025.UI;

public partial class GameController : Node
{
	[Export] private UIHUD hud;
	public bool isPaused { get; private set; }
	public override void _Process(double delta)
	{
		base._Process(delta);
		if (Input.IsActionJustPressed("Pause"))
		{
			if (isPaused)
			{
				Unpause();
			}
			else
			{
				Pause();
			}
		}
	}

	private void Pause()
	{
		isPaused = true;
		hud.Pause();
	}

	private void Unpause()
	{
		isPaused = false;
		hud.Unpause();
	}
}

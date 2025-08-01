using Godot;
using System;
using System.Linq;
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
		GD.Print("PAUSE");
		isPaused = true;
		hud.Pause();
		GetTree().Paused = true;
	}

	private void Unpause()
	{
		GD.Print("UNPAUSE");
		isPaused = false;
		hud.Unpause();
		GetTree().Paused = false;
	}
}

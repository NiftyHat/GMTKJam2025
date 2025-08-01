using Godot;
using System;
using System.Linq;
using GMTKJam2025.UI;

public partial class GameController : Node
{
	[Export] private UIHUD hud;
	[Export] private PauseScreen pauseScreen;
	
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

	public void Reset()
	{
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}

	public void Pause()
	{
		isPaused = true;
		pauseScreen.Activate();
		GetTree().Paused = true;
	}

	public void Unpause()
	{
		isPaused = false;
		pauseScreen.Dismiss();
		GetTree().Paused = false;
	}

	public void QuitToTitle()
	{
		GetTree().Paused = false;
		SceneSwitcher.Instance.GoToScene(SceneSwitcher.Instance.Library.Title);
	}
}

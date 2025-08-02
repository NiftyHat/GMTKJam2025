using Godot;
using System;
using GMTKJam2025.Audio;
using GMTKJam2025.UI;

public partial class EndSceneBehaviour : Node
{
	[Export] private Button Restart;
	[Export] private Button MainMenu;

	public override void _Ready()
	{
		MusicPlayer.Instance.Stop();
		base._Ready();
		Restart.GrabFocus();
	}

	public void RestartLevel()
	{
		if (GameInformation.CurrentLevel == null)
		{
			GotoMainMenu();
			return;
		}
		SceneSwitcher.Instance.GoToScene(GameInformation.CurrentLevel);
	}
	public void GotoMainMenu()
	{
		SceneSwitcher.Instance.GoToScene(SceneSwitcher.Instance.Library.Title);
	}
}

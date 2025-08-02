using Godot;
using System;
using System.Linq;
using GMTKJam2025.UI;
using GMTKJam2025.UI.MainMenu;

public partial class MainMenuControl : Node
{
	private IMenuDisplay[] allScreens;

	private MainMenuTitleDisplay TitleScreen;
	private MainMenuMainMenuDisplay MenuScreen;
	private MainMenuTextScreenPopupDisplay HowToPlayScreen;
	private MainMenuTextScreenPopupDisplay CreditsScreen;

	public override void _Ready()
	{
		
		base._Ready();
		
		TitleScreen = (MainMenuTitleDisplay)GetNode("TitleScreenDisplay");
		MenuScreen = (MainMenuMainMenuDisplay)GetNode("MainMenuDisplay");
		HowToPlayScreen = (MainMenuTextScreenPopupDisplay)GetNode("HowToPlayDisplay");
		CreditsScreen = (MainMenuTextScreenPopupDisplay)GetNode("CreditsDisplay");

		allScreens = GetChildren().Where(x => x is IMenuDisplay).Select(x => x as IMenuDisplay).ToArray();
		DeprocessAllScreens();
	
		foreach (var screen in allScreens)
		{
			screen.HideScreen();
		}
		
		ShowTitleScreen();
	}

	private void DeprocessAllScreens()
	{		
		foreach (var screen in allScreens)
		{
			screen.Lock();
		}
	}

	public void ShowTitleScreen()
	{
		DeprocessAllScreens();
		
		TitleScreen.Unlock();
		TitleScreen.SetProcess(true);

		MenuScreen.HideScreen();
		
		TitleScreen.Start();
	}

	public void ShowMenuScreen()
	{
		DeprocessAllScreens();
		
		MenuScreen.Unlock();
		MenuScreen.SetProcess(true);
		
		TitleScreen.HideScreen();

		MenuScreen.Start();
	}

	public void ReturnToMainMenu()
	{
		DeprocessAllScreens();
		MenuScreen.Unlock();
		MenuScreen.RestoreFocus();
	}

	public void ShowHowToPlay()
	{
		MenuScreen.SaveFocus();
		DeprocessAllScreens();
		HowToPlayScreen.SetProcess(true);
		
		GD.Print("How to Play");
		HowToPlayScreen.Start();
	}
	
	public void ShowTrackSelect()
	{
		GD.Print("Track Select");
		GameInformation.CurrentLevel = SceneSwitcher.Instance.Library.Game;
		SceneSwitcher.Instance.GoToScene(SceneSwitcher.Instance.Library.Game);
	}
	public void ShowOptions()
	{
		GD.Print("Options");
	}
	public void ShowCredits()
	{
		MenuScreen.SaveFocus();
		DeprocessAllScreens();
		CreditsScreen.SetProcess(true);
		
		GD.Print("Credits");
		CreditsScreen.Start();
	}
	public void ShowRecords()
	{
		GD.Print("Records");
	}
}

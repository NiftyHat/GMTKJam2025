using Godot;
using System;
using System.Linq;
using GMTKJam2025.UI;
using GMTKJam2025.UI.MainMenu;

public partial class TrackSelectScreenDisplay :  Control, IMenuDisplay
{

	[Signal] public delegate void CancelEventHandler();
	[Export] private Button[] Buttons;
	[Export] private PackedScene[] Levels;
	
	public void Start()
	{
		Modulate = Colors.White;
		
		Buttons.ToList().ForEach(x => x.Visible = true);
		Unlock();
		
		Buttons[0].GrabFocus();
	}

	public void HideScreen()
	{
		Lock();
		
		Buttons.ToList().ForEach(x => x.Visible = false);
		Modulate = Colors.Transparent;
	}

	public void Lock()
	{
		SetProcess(false);
		Buttons.ToList().ForEach(x => x.Disabled = true);
	}

	public void Unlock()
	{
		SetProcess(true);
		Buttons.ToList().ForEach(x => x.Disabled = false);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		
		base._Process(delta);
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			HideScreen();
			EmitSignalCancel();
		}
	}

	public void LoadLevel_0() => LoadLevel(0);
	public void LoadLevel_1() => LoadLevel(1);
	public void LoadLevel_2() => LoadLevel(2);
	public void LoadLevel_3() => LoadLevel(3);

	private void LoadLevel(int i)
	{
		GameInformation.CurrentLevel = Levels[i];
		SceneSwitcher.Instance.GoToScene(Levels[i]);
	}
}

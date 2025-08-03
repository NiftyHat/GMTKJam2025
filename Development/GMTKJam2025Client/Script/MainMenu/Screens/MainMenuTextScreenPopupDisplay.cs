using Godot;
using System;
using GMTKJam2025.UI.MainMenu;

public partial class MainMenuTextScreenPopupDisplay : Control, IMenuDisplay
{
	[Signal] public delegate void CancelEventHandler();
	
	public void Start()
	{
		Modulate = Colors.White;
	}
	
	public void Lock()
	{
		SetProcess(false);
	}
	
	public void Unlock()
	{
		SetProcess(true);
	}

	public void HideScreen()
	{
		Modulate = Colors.Transparent;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			HideScreen();
			EmitSignalCancel();
		}
	}

	public void LoadHyperLink(Variant meta)
	{
		OS.ShellOpen((String)meta);
	}
}

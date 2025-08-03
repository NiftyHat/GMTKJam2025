using Godot;
using System;
using System.Linq;
using GMTKJam2025.UI.MainMenu;

public partial class MainMenuTextScreenPopupDisplay : Control, IMenuDisplay
{
	[Signal] public delegate void CancelEventHandler();
	
	public void Start()
	{
		Modulate = Colors.White;
		FindChildren("RichTextLabel").Select(x => x as RichTextLabel).ToList().ForEach(x => x.Visible = true);
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
		FindChildren("RichTextLabel").Select(x => x as RichTextLabel).ToList().ForEach(x => x.Visible = false);
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

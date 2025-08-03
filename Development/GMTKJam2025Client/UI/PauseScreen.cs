using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PauseScreen : CanvasLayer
{
	[Signal] public delegate void PauseScreenActivatedEventHandler();
	[Signal] public delegate void PauseScreenDismissedEventHandler();
	
	[Signal] public delegate void ButtonRestartEventHandler();
	[Signal] public delegate void ButtonContinueEventHandler();
	[Signal] public delegate void ButtonAudioEventHandler();
	[Signal] public delegate void ButtonControlsEventHandler();
	[Signal] public delegate void ButtonFullscreenEventHandler();
	[Signal] public delegate void ButtonMainMenuEventHandler();
	
	
	private List<Button> buttons;
	public override void _Ready()
	{
		base._Ready();
		buttons = GetNode("PanelContainer/MarginContainer/VBoxContainer").GetChildren()
			.Where(x => x is Button)
			.Select(x => x as Button)
			.ToList();
		buttons.Sort((a, b) => a.Position.Y < b.Position.Y ? -1 : 1);
		
		Visible = false;
		buttons.ForEach(x => x.Disabled = true);
	}

	public void Activate()
	{
		Visible = true;
		buttons.ForEach(x => x.Disabled = false);
		buttons[0].GrabFocus();
		EmitSignalPauseScreenActivated();
	}

	public void Dismiss()
	{
		Visible = false;
		buttons.ForEach(x => x.Disabled = true);
		EmitSignalPauseScreenDismissed();
	}

	public void OnContinue() => EmitSignalButtonContinue();
	public void OnReset() => EmitSignalButtonRestart();
	public void OnMainMenu() => EmitSignalButtonMainMenu();

}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GMTKJam2025.UI.MainMenu;

public partial class MainMenuMainMenuDisplay : Control, IMenuDisplay
{
	
	[Signal] public delegate void CancelEventHandler();

	private Tween AnimationTween;
	private List<Button> buttons;
	float ButtonPos = 862.0f;
	float OffScreen = 862.0f;

	private Control focus;

	public override void _Ready()
	{
		base._Ready();

		buttons = GetNode("MarginContainer/VBoxContainer").GetChildren()
			.Where(x => x is Button)
			.Select(x => x as Button)
			.ToList();
		buttons.Sort((a, b) => a.Position.Y < b.Position.Y ? -1 : 1);
		ButtonPos = buttons[0].Position.X;
		
		OffScreen = GetViewport().GetWindow().Size.X;
	}

	public void Start()
	{
		OffScreen = GetViewport().GetWindow().Size.X;
		focus = null;
		
		AnimationTween = CreateTween();
		
		EnableScreen();
		const float animationSpeed = 0.24f;
		const float delay = 0.12f;
		
		for (int i = 0; i < buttons.Count(); i++)
		{
			var b = buttons[i];
			b.Position = new Vector2(OffScreen, b.Position.Y);
			
			var t = CreateTween();
			t.TweenInterval(i * delay); 
			t.TweenProperty(b, "position:x", ButtonPos, animationSpeed);
			AnimationTween.Parallel().TweenSubtween(t);
		}
		
		Modulate = Colors.White;
		buttons[0].GrabFocus();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			CancelScreen();
		}
	}

	private void DisableScreen() => buttons.ForEach(x => x.Disabled = true);
	private void EnableScreen() => buttons.ForEach(x => x.Disabled = false);

	public void Lock()
	{
		SetProcess(false);
		buttons.ForEach(x => x.Disabled = true);
	}

	public void SaveFocus()
	{
		focus = null;
		buttons.ForEach(x =>
		{
			x.Disabled = true;
			if (x.HasFocus()) focus = x;
		});
	}

	public void Unlock()
	{
		SetProcess(true);
		buttons.ForEach(x => x.Disabled = false);
	}

	private void CancelScreen()
	{
		DisableScreen();
		EmitSignalCancel();
	}

	public void HideScreen()
	{
		DisableScreen();
		Modulate = Colors.Transparent;
	}

	public void RestoreFocus()
	{
		if (focus != null)
		{
			focus.GrabFocus();
		}
	}
}

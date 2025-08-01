using Godot;
using System;
using Godot.Collections;

public partial class MainMenuController : Control
{
	public enum State
	{
		TITLE = 0,
		MENU = 1
	}

	private State camerastate = State.TITLE; 
	
	[Export] private GameCamera GameCamera;
	[Export] private Dictionary<State, Node3D> CameraPositionsByState;
	[Export] private Dictionary<State, Node3D> CameraFocusesByState;

	private MainMenuTitleDisplay _mainMenuTitleDisplay;

	public override void _Ready()
	{
		base._Ready();
		ChangeCamera( State.TITLE);
	}

	public void ChangeCamera(State statex)
	{
		camerastate = statex;
		GD.Print($"Camera Moving to {camerastate}");
		GameCamera.CameraPositionTarget = CameraPositionsByState[camerastate];
		GameCamera.CameraFocusTarget = CameraFocusesByState[camerastate];
	}

	public void ChangeCameraToTitle() => ChangeCamera(State.TITLE);
	public void ChangeCameraToMenu() => ChangeCamera(State.MENU);
	
}

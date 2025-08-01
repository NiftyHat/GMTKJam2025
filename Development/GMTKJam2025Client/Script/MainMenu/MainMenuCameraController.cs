using Godot;
using System;
using System.Collections.Generic;

public partial class MainMenuCameraController : Control
{
	public enum State
	{
		TITLE = 0,
		MENU = 1
	}

	private State camerastate = State.TITLE; 
	
	[Export] public GameCamera GameCamera;

	[Export] public Node3D TitlePosition;
	[Export] public Node3D TitleFocus;
	[Export] public float TargetTitleFOV;
	[Export] public Node3D MenuPosition;
	[Export] public Node3D MenuFocus;
	[Export] public float TargetMenuFOV;

	private MainMenuTitleDisplay _mainMenuTitleDisplay;

	public void ChangeCamera(State statex)
	{
		camerastate = statex;
		GD.Print($"Camera Moving to {camerastate}");
		GameCamera.CameraPositionTarget = camerastate switch
		{
			State.TITLE => TitlePosition,
			State.MENU => MenuPosition,
			_ => MenuPosition
		};
		GameCamera.CameraFocusTarget = camerastate switch
		{
			State.TITLE => TitleFocus,
			State.MENU => MenuFocus,
			_ => MenuFocus
		};
		GameCamera.FOVTarget = camerastate switch
		{
			State.TITLE => TargetTitleFOV,
			State.MENU => TargetMenuFOV,
			_ => TargetMenuFOV
		};
	}

	public void ChangeCameraToTitle() => ChangeCamera(State.TITLE);
	public void ChangeCameraToMenu() => ChangeCamera(State.MENU);
	
}

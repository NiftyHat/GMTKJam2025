using Godot;
using System;

public partial class GameCamera : Camera3D
{
	[Export] private Node3D LookAtTarget;
	[Export] public Node3D CameraPositionTarget;
	[Export] public Node3D CameraFocusTarget;

	[Export] public float SnapPositionSpeed = 0.15f;
	[Export] public float SnapFocusSpeed = 0.05f;

	[Export] public float FOVAtMaxSpeed = 110f;
	[Export] public float FOVNormal = 75f;
	[Export] public float FOVTarget = 75f;

	[Export] public CarController CarController;

	public override void _Ready()
	{
		base._Ready();
		Fov = FOVNormal;
		FOVTarget = FOVNormal;
		if (CameraPositionTarget != null) GlobalPosition = CameraPositionTarget.GlobalPosition;
		if (LookAtTarget != null)
		{
			LookAtTarget.GlobalPosition = CameraFocusTarget.GlobalPosition;
			LookAt(LookAtTarget.GlobalPosition);
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		
		// Camera
		if (CameraPositionTarget != null)
		{
			Position = Position.Lerp(CameraPositionTarget.GlobalPosition, SnapPositionSpeed);
		}
		if (CameraFocusTarget != null)
		{
			LookAtTarget.Position = LookAtTarget.Position.Lerp(CameraFocusTarget.GlobalPosition, SnapFocusSpeed);
		}

		if (CarController != null)
		{
			FOVTarget = FOVNormal + (FOVAtMaxSpeed - FOVNormal) * CarController.Speed / CarController.MaxSpeed;
		}

		Fov = float.Lerp(Fov, FOVTarget, (float)delta);

		LookAt(LookAtTarget.GlobalPosition);
	}
}

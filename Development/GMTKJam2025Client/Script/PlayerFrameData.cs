using Godot;

namespace GMTKJam2025;

public struct PlayerFrameData
{
	public Transform3D Transform;
	public float TurnAngleInRadians;
	public float SpeedInput;
	public CarController.BrakeState BrakeState;
	public bool IsGrounded;
	public float Speed;
	public float Velocity;
	public Vector3 Drift;
}
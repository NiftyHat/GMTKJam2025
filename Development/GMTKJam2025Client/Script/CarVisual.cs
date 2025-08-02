using Godot;
using System;

public partial class CarVisual: Node3D
{
	private Node3D SteeringL;
	private Node3D SteeringR;
	private Node3D Driver;

	private Vector3 SteeringLOrigin;
	private Vector3 SteeringROrigin;
	private Vector3 DriverOrigin;

	private Vector3 DriverRotation;
	private Vector3 WheelSteeringRotation;
	private Vector3 WheelAxialRotation;
	
	public override void _Ready()
	{
		base._Ready();
		SteeringL = (Node3D)FindChild("BackLeftWheel_*");
		SteeringR = (Node3D)FindChild("BackRightWheel_*");
		Driver = (Node3D)FindChild("Driver*");

		SteeringLOrigin = SteeringL.Rotation;
		SteeringROrigin = SteeringR.Rotation;
		DriverOrigin = Driver.Rotation;

		// demo...
		//SteeringL.Scale = Vector3.One * 4;
		//SteeringR.Scale = Vector3.One * 4;
		
		WheelAxialRotation = Vector3.Zero;

	}

	public override void _Process(double delta)
	{
		const float slerpWeight = 0.1f;
		
		SteeringL.Rotation = SteeringL.Rotation.Slerp(WheelSteeringRotation + WheelAxialRotation + SteeringLOrigin, slerpWeight);
		SteeringR.Rotation = SteeringR.Rotation.Slerp(WheelSteeringRotation + WheelAxialRotation + SteeringROrigin, slerpWeight);
		Driver.Rotation = Driver.Rotation.Slerp(DriverRotation + DriverOrigin, slerpWeight);
	}
	public void Steer(float angleInRadians)
	{
		float steerWeight = angleInRadians * 5;
		float driverWeight = -angleInRadians * 5;

		WheelSteeringRotation = -new Vector3(steerWeight, 0, 0);
		DriverRotation = new Vector3(driverWeight, 0, 0);
		
	}
}

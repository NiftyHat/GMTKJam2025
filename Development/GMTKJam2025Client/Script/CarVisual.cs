using Godot;
using System;

public partial class CarVisual: Node3D
{
	private Node3D SteeringL;
	private Node3D SteeringR;
	private Node3D DriveL;
	private Node3D DriveR;
	private Node3D Driver;

	private Vector3 SteeringLOrigin;
	private Vector3 SteeringROrigin;
	private Vector3 DriverOrigin;
	private Vector3 WheelAxialRotation;
	
	private Vector3 DriftDirection;
	private Vector3 WheelSteeringRotation;
	private Vector3 DriverRotation;

	[Export] private GpuParticles3D driftSmokeA;
	[Export] private GpuParticles3D driftSmokeB;
	private GpuParticles3D trail;

	//private readonly Vector3 TrailPosition = new Vector3(0, -0.769f, 2.508f);
	private readonly Vector3 TrailPosition = new Vector3(0, -0.89f, 2.508f);
	
	public override void _Ready()
	{
		base._Ready();
		DriveL = (Node3D)FindChild("FrontLeftWheel_*");
		DriveR = (Node3D)FindChild("FrontRightWheel_*");
		SteeringL = (Node3D)FindChild("BackLeftWheel_*");
		SteeringR = (Node3D)FindChild("BackRightWheel_*");
		Driver = (Node3D)FindChild("Driver*");

		SteeringLOrigin = SteeringL.Rotation;
		SteeringROrigin = SteeringR.Rotation;
		DriverOrigin = Driver.Rotation;
		
		// demo...
		WheelAxialRotation = Vector3.Zero;

	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public void Steer(float angleInRadians)
	{
		const float slerpWeight = 0.1f;
		float steerWeight = angleInRadians * 3;
		float driverWeight = -angleInRadians * 5;
		
		
		SteeringL.Rotation = SteeringL.Rotation.Slerp(new Vector3(steerWeight, float.DegreesToRadians(-180), float
            .DegreesToRadians(90)), slerpWeight);
		SteeringR.Rotation = SteeringR.Rotation.Slerp( new Vector3(steerWeight, float.DegreesToRadians(-180), float.DegreesToRadians(90)), slerpWeight);
		Driver.Rotation = Driver.Rotation.Slerp(new Vector3(driverWeight, 0, float.DegreesToRadians(90)), slerpWeight);
		WheelSteeringRotation = -new Vector3(steerWeight, 0, 0);
		DriverRotation = new Vector3(driverWeight, 0, 0);
	}

	public void Drift(Vector3 v = new Vector3())
	{
		driftSmokeA.GlobalPosition = SteeringL.GlobalPosition;
		driftSmokeB.GlobalPosition = DriveL.GlobalPosition;

		DriftDirection = v;
		bool isEmitting = DriftDirection.Length() > 0;
		driftSmokeB.Emitting = driftSmokeA.Emitting = DriftDirection.Length() > 0;

		if (isEmitting)
		{
			if (DriftDirection.Dot(GlobalBasis.X) < 0)
			{
				driftSmokeB.GlobalPosition = DriveL.GlobalPosition;
				driftSmokeA.GlobalPosition = SteeringL.GlobalPosition;
			}
			else
			{
				driftSmokeB.GlobalPosition = DriveR.GlobalPosition;
				driftSmokeA.GlobalPosition = SteeringR.GlobalPosition;
			}
		}
	}
}

using Godot;
using System;

public partial class CarVisual: Node3D
{
	private Node3D SteeringL;
	private Node3D SteeringR;
	private Node3D Driver;
	
	public override void _Ready()
	{
		base._Ready();
		SteeringL = (Node3D)FindChild("BackLeftWheel_*");
		SteeringR = (Node3D)FindChild("BackRightWheel_*");
		Driver = (Node3D)FindChild("Driver*");
		
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
	}
}

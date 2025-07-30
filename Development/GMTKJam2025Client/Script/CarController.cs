using Godot;
using System;
using System.Linq;

public partial class CarController : Node3D
{
	public enum BrakeState
	{
		NOT,
		BRAKE,
		REVERSE
	}

	
	public float Speed => _rb.LinearVelocity.Dot(Transform.Basis.Z);
	public float Velocity => _rb.LinearVelocity.Length();
	public BrakeState _brakeState { get; private set; } = BrakeState.NOT;
	
	
	public float speedInput { get; private set; } = 0;
	public float turnInput { get; private set; } = 0;
	public bool isGrounded { get; private set; } = false;
	
	[Export] private RigidBody3D _rb;
	[Export] private Camera3D _camera3D;
	
	[Export] private Node3D CameraLookTarget;
	[Export] private Node3D CameraPositionTarget;
	[Export] private Node3D GroundCheckNode;
	
	[Export] private float Acceleration;
	[Export] private float BrakeStrength;
	[Export] private float reverseAcceleration;
	[Export] private float maxSpeed;
	[Export] private float turnStrength;
	[Export] private float GroundDistance;
	[Export] private float Downforce;
	
	private const float M = 1000f;
	
	public override void _Process(double delta)
	{
		base._Process(delta);
		Position = _rb.Position;
		
		
		// Acceleration, Braking, and Reversing vroom
		GD.Print($"Speed : {Speed}");

		speedInput = 0;
		float vInput = Input.GetAxis("Brake", "Accelerate");
		if (Math.Abs(vInput) != 0)
		{
			if (vInput > 0)
			{
				// accelerate if below maxspeed
				speedInput = vInput * Acceleration * M;
				_brakeState = BrakeState.NOT;
			}
			else
			{
				// brake if currentspeed > 0 if below maxspeed
				if (Speed > 0 && _brakeState != BrakeState.REVERSE)
				{
					speedInput = vInput * BrakeStrength * M;
					_brakeState = BrakeState.BRAKE;
				}
				else if(_brakeState != BrakeState.BRAKE){
					// otherwise, reverse
					speedInput = vInput * reverseAcceleration * M;
					_brakeState = BrakeState.REVERSE;
				}
			} 
		} else if(Mathf.Abs(Speed) < 0.05f)
		{	
			_brakeState = BrakeState.NOT;
		}

		// Turning; capped by velocity
		turnInput = Input.GetAxis("SteerLeft", "SteerRight");
		Rotate(Vector3.Up, -turnInput * turnStrength * (float)delta * float.Clamp(Velocity * 0.5f, -1, 1));
		
		
		
		// Camera
		var lookatraycast = PhysicsRayQueryParameters3D.Create(CameraLookTarget.GlobalPosition, CameraLookTarget.GlobalPosition + Vector3.Down * 1000);
		var result = GetWorld3D().DirectSpaceState.IntersectRay(lookatraycast);
		
		
		_camera3D.Position = _camera3D.Position.Lerp(CameraPositionTarget.GlobalPosition, .15f);
		_camera3D.LookAt(CameraLookTarget.GlobalPosition);
	}

	public override void _PhysicsProcess(double delta)
	{
		// check groundedness
		isGrounded = false;
		var raycast = PhysicsRayQueryParameters3D.Create(GroundCheckNode.GlobalPosition, GroundCheckNode.GlobalPosition + Vector3.Down * GroundDistance);
		var result = GetWorld3D().DirectSpaceState.IntersectRay(raycast);

		isGrounded = result.Count > 0;
		
		base._PhysicsProcess(delta);
		if (isGrounded)
		{
			// Allow Drive
			if (Math.Abs(speedInput) > 0)
			{
				_rb.ApplyForce(Transform.Basis * new Vector3(0, 0, speedInput));
			}

			// Control Drive Speed if > Max Speed
			// Check it works with gravity?
			if (Speed > maxSpeed)
			{
				_rb.LinearVelocity *= maxSpeed / Speed;
			}
		}
		else
		{
			// apply extra downforce to stop floatiness
			_rb.ApplyForce(Transform.Basis * new Vector3(0, -Downforce * M, 0));
		}
	}
}

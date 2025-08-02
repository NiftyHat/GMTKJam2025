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
	public Vector3 DriftAngle { get; private set; } = Vector3.Zero;
	
	[Export] private RigidBody3D _rb;
	[Export] private Camera3D _camera3D;
	[Export] private CarVisualLibrary _carVisualLibrary;
	
	private Node3D CameraLookTarget;
	private Node3D CameraPositionTarget;
	private RayCast3D GroundCheckRaycast;

	private CarVisual CarVisualNode { get; set; }

	[Export] private float Acceleration;
	[Export] private float BrakeStrength;
	[Export] private float reverseAcceleration;
	[Export] public float MaxSpeed { get; private set; }
	[Export] private float maxReverseSpeed;
	[Export] private float steering;
	[Export] private float turnSpeed;
	[Export] private float minimumTurnSpeed;
	
	private const float M = 1000f;

	public override void _Ready()
	{
		CameraPositionTarget = GetNode<Node3D>("Ideal Camera Location");
		CameraLookTarget = GetNode<Node3D>("Camera Look At Target");
		GroundCheckRaycast = GetNode<RayCast3D>("GroundCheckRaycast");
		GroundCheckRaycast.AddException(_rb);

		Transform = _rb.Transform;
		base._Ready();

		ChangeVisuals(1);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		var p = Position;
		p.X = float.Lerp(Position.X, _rb.Position.X, 0.9f);
		p.Y = float.Lerp(Position.Y, _rb.Position.Y, 0.4f);
		p.Z = float.Lerp(Position.Z, _rb.Position.Z, 0.9f);
		Position = p;


		// Player Input
		
		
		// Acceleration, Braking, and Reversing vroom

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
		turnInput = Mathf.DegToRad(Input.GetAxis("SteerLeft", "SteerRight") * steering);
		
		// Visuals
		CarVisualNode.Steer(turnInput);

		if (isGrounded && Velocity > 10 && (Speed < Velocity * .5f))
		{
			DriftAngle = _rb.LinearVelocity;
		}
		else
		{
			DriftAngle = Vector3.Zero;
		}
		CarVisualNode.Drift(DriftAngle);
		
	}

	public override void _PhysicsProcess(double delta)
	{
		// check groundedness
		isGrounded = GroundCheckRaycast.IsColliding();
		
		base._PhysicsProcess(delta);

		Transform3D aimTransform;
		if (isGrounded)
		{
			// Allow Drive
			if (Math.Abs(speedInput) > 0)
			{
				_rb.ApplyCentralForce(Transform.Basis * new Vector3(0, 0, speedInput));
			}

			// Control Drive Speed if > Max Speed
			// Check it works with gravity?
			if (Speed > MaxSpeed)
			{
				_rb.LinearVelocity *= MaxSpeed / Speed;
			} else if (Speed < maxReverseSpeed)
			{
				_rb.LinearVelocity *= maxReverseSpeed / Speed;
			}

			if (_rb.LinearVelocity.Length() > minimumTurnSpeed)
			{
				Rotate(Vector3.Up, -turnInput * turnSpeed * (float)delta);
			}


			aimTransform = AlignWithY(GlobalTransform, GroundCheckRaycast.GetCollisionNormal().Normalized());
			
		}
		else
		{
			aimTransform = AlignWithY(GlobalTransform, Vector3.Up);
		}
		
		GlobalTransform = GlobalTransform.InterpolateWith(aimTransform, (float)delta * 10f) ;
	}

	private Transform3D AlignWithY(Transform3D t, Vector3 v)
	{
		t.Basis.Y = v;
		t.Basis.X = t.Basis.Z.Cross(v);
		t.Basis = t.Basis.Orthonormalized();
		return t;
	}

	public void ChangeVisuals(int lap)
	{
		if (CarVisualNode != null)
		{
			RemoveChild(CarVisualNode);
			CarVisualNode.Free();
		}
		CarVisualNode = _carVisualLibrary.GetNewCar(lap-1);
		AddChild(CarVisualNode);
	}
}

using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GMTKJam2025;
using Godot.NativeInterop;

public partial class GhostRecorder : Node
{
	[Export] private CarController CarRigidbody;
	[Export] private Node3D GhostCarPrototype;
	[Export] private CarVisualLibrary CarVisualLibrary;
	
	private List<PlayerFrameData> CurrentRecording;
	private List<List<PlayerFrameData>> AllRecordings;

	private List<Node3D> GhostCars;
	private List<Node3D> GhostCarsContainers;
	private List<CarVisual> GhostCarVisuals;

	public bool Recording = false;
	public int currentFrame = 0;
	
	public override void _Ready()
	{
		base._Ready();
		CurrentRecording = new List<PlayerFrameData>();
		AllRecordings = new List<List<PlayerFrameData>>();
		GhostCars = new List<Node3D>();
		GhostCarsContainers = new List<Node3D>();
		GhostCarVisuals = new List<CarVisual>();
	}

	public void Lap()
	{
		GD.Print($"Lap Length: {CurrentRecording.Count}");

		var newCar = (Node3D)GhostCarPrototype.Duplicate();
		GhostCars.Add(newCar);
		AddChild(newCar);
		var newCarVisual = CarVisualLibrary.GetNewCar(AllRecordings.Count);
		GhostCarVisuals.Add(newCarVisual);
		newCar.AddChild(newCarVisual);
		
		newCar.Transform = CurrentRecording[0].Transform;
		AllRecordings.Add(CurrentRecording.ToList());

		currentFrame = 0;
		CurrentRecording = new List<PlayerFrameData>();
		
		int ghost = 0;
		for (; ghost < AllRecordings.Count; ghost++)
		{
			var currentRecording = AllRecordings[ghost];
			var car = GhostCars[ghost];
			if(car.GetParent() == null) AddChild(car);
			car.SetProcess(true);
			car.Transform = currentRecording[0].Transform;
		}

		GD.Print($"Lap Count, {AllRecordings.Count}");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		
		if(!Recording) return;
		
		CurrentRecording.Add(new PlayerFrameData()
		{
			Transform = CarRigidbody.GlobalTransform,
			SpeedInput = CarRigidbody.Speed,
			BrakeState = CarRigidbody._brakeState,
			TurnAngleInRadians = CarRigidbody.turnInput,
			IsGrounded = CarRigidbody.isGrounded,
			Speed = CarRigidbody.Speed,
			Velocity = CarRigidbody.Velocity,
			Drift = CarRigidbody.DriftAngle
		});

		currentFrame++;

		int ghost = 0;
		if (currentFrame == 100)
		{
			GD.Print("Removing Crates");
			foreach (var container in GhostCarsContainers)
			{
				container.GetParent().RemoveChild(container);
			}
		}
		else if (currentFrame == 200)
		{
			GD.Print("Adding Crates");
			for (ghost = 0; ghost < AllRecordings.Count; ghost++)
			{
				AddChild(GhostCarsContainers[ghost]);
				GhostCarsContainers[ghost].Transform = AllRecordings[ghost][0].Transform;
			}
			
			var newCrate = CarVisualLibrary.GetNewCrate(AllRecordings.Count);
			GhostCarsContainers.Add(newCrate);
			AddChild(newCrate);
			newCrate.Transform = CurrentRecording[0].Transform;
		}
		
		for(ghost = 0; ghost < AllRecordings.Count; ghost++)
		{
			var currentRecording = AllRecordings[ghost];
			var car = GhostCars[ghost];
			var carVisual = GhostCarVisuals[ghost];
			
			if(car.GetParent() == null) continue;

			if (currentRecording.Count <= currentFrame)
			{
				car.GetParent().RemoveChild(car);
				car.SetProcess(false);
				continue;
			}

			var currentFrameData = currentRecording[currentFrame];
				
			car.Transform = currentFrameData.Transform;

			// Visuals
			car.Transform = currentFrameData.Transform;
			carVisual.Steer(currentFrameData.TurnAngleInRadians);
			carVisual.Drift(currentFrameData.Drift);
		}
	}

	public void TriggerRecording()
	{
		if (!Recording)
		{
			GD.Print("Recording Start");
			Recording = true;
			currentFrame = 0;
		}
		else
		{
				
			Lap();
				
		}
	}
}

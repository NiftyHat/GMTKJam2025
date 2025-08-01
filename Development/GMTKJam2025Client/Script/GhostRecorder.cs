using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot.NativeInterop;

public partial class GhostRecorder : Node
{
	[Export] private Node3D CarRigidbody;
	[Export] private Node3D GhostCarPrototype;
	[Export] private Node3D GhostCarContainerPrototype;
	
	private List<Transform3D> CurrentRecording;
	private List<List<Transform3D>> AllRecordings;

	private List<Node3D> GhostCars;
	private List<Node3D> GhostCarsContainers;

	public bool Recording = false;
	public int currentFrame = 0;
	
	

	public override void _Ready()
	{
		base._Ready();
		CurrentRecording = new List<Transform3D>();
		AllRecordings = new List<List<Transform3D>>();
		GhostCars = new List<Node3D>();
		GhostCarsContainers = new List<Node3D>();
	}

	public void Lap()
	{
		GD.Print($"Lap Length: {CurrentRecording.Count}");

		var newCar = (Node3D)GhostCarPrototype.Duplicate();
		GhostCars.Add(newCar);
		AddChild(newCar);
		
		newCar.Transform = CurrentRecording[0];
		AllRecordings.Add(CurrentRecording.ToList());

		currentFrame = 0;
		CurrentRecording = new List<Transform3D>();
		
		int ghost = 0;
		for (; ghost < AllRecordings.Count; ghost++)
		{
			var currentRecording = AllRecordings[ghost];
			var car = GhostCars[ghost];
			if(car.GetParent() == null) AddChild(car);
			car.SetProcess(true);
			car.Transform = currentRecording[0];
		}

		GD.Print($"Lap Count, {AllRecordings.Count}");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		
		if(!Recording) return;
		CurrentRecording.Add(CarRigidbody.GlobalTransform);

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
				GhostCarsContainers[ghost].Transform = AllRecordings[ghost][0];
			}
			
			var newCrate = (Node3D)GhostCarContainerPrototype.Duplicate();
			GhostCarsContainers.Add(newCrate);
			AddChild(newCrate);
			newCrate.Transform = CurrentRecording[0];
		}
		
		for(ghost = 0; ghost < AllRecordings.Count; ghost++)
		{
			var currentRecording = AllRecordings[ghost];
			var car = GhostCars[ghost];
			
			if(car.GetParent() == null) continue;

			if (currentRecording.Count <= currentFrame)
			{
				car.GetParent().RemoveChild(car);
				car.SetProcess(false);
				continue;
			}

			car.Transform = currentRecording[currentFrame];

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

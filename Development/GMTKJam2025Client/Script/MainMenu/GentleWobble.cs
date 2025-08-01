using Godot;
using System;

public partial class GentleWobble : Node3D
{
	[Export] private Vector3 Range;
	[Export] private float Duration;
	private Tween motion;
	public override void _Process(double delta)
	{
		base._Process(delta);
		if (motion == null || !motion.IsRunning())
		{

			Vector3 newPos = new Vector3(Random.Shared.NextSingle(), Random.Shared.NextSingle(), Random.Shared
				.NextSingle()) * Range;

			motion = CreateTween();
			motion.TweenProperty(this, "position", newPos, Duration);
		}
	}
}

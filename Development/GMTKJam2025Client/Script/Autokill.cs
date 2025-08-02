using Godot;
using System;

public partial class Autokill : Node
{
	public override void _Ready()
	{
		base._Ready();
		Free();
	}
}

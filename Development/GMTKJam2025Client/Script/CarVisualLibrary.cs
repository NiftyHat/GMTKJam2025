using Godot;
using System;
using System.Linq;

public partial class CarVisualLibrary : Node
{
	private CarVisual[] carPrototypes;
	public override void _Ready()
	{
		base._Ready();
		var carPrototypesList = GetChildren().Where(x => x is CarVisual).Select(x => x as CarVisual).ToList();
		carPrototypesList.Sort((a, b) => String.Compare(a.Name, b.Name));
		carPrototypes = carPrototypesList.ToArray();
		GD.Print($"Car Count: {carPrototypes.Length}");
	}

	public CarVisual GetNewCar(int lap)
	{
		GD.Print($"GETTING CAR {lap}");
		return (CarVisual)(carPrototypes[lap % carPrototypes.Length].Duplicate());
	}
}

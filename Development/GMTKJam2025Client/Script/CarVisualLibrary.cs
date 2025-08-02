using Godot;
using System;
using System.Linq;

public partial class CarVisualLibrary : Node
{
	private CarVisual[] carPrototypes;
	private CrateVisual[] cratePrototypes;
	public override void _Ready()
	{
		base._Ready();
		var children = GetChildren();
		var carPrototypesList = children.Where(x => x is CarVisual).Select(x => x as CarVisual).ToList();
		carPrototypesList.Sort((a, b) => String.Compare(a.Name, b.Name));
		carPrototypes = carPrototypesList.ToArray();
		
		var cratePrototypesList = children.Where(x => x is CrateVisual).Select(x => x as CrateVisual).ToList();
		cratePrototypesList.Sort((a, b) => String.Compare(a.Name, b.Name));
		cratePrototypes = cratePrototypesList.ToArray();
		
		GD.Print($"Car Count: {carPrototypes.Length}");
	}

	public CarVisual GetNewCar(int lap)
	{
		return (CarVisual)(carPrototypes[lap % carPrototypes.Length].Duplicate());
	}
	public CrateVisual GetNewCrate(int lap)
	{
		return (CrateVisual)(cratePrototypes[lap % cratePrototypes.Length].Duplicate());
	}
}

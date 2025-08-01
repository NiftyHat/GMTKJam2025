using Godot;

namespace GMTKJam2025.UI;

[GlobalClass]
public partial class SceneLibraryData : Resource
{
    [Export] public PackedScene Title { get; set; }
    [Export] public PackedScene GameOver { get; set; }
    [Export] public PackedScene Game { get; set; }
}
using Godot;

namespace GMTKJam2025.UI;

public partial class SceneSwitcher : Node
{
    public SceneLibraryData Library { get; private set; }
    const string sceneLibraryPath = "res://scene_library.tres";
    public static SceneSwitcher Instance { get; private set; }
    
    public override void _Ready()
    {
        if (Instance != null)
        {
            GD.PushError("Instance already exists");
            this.QueueFree();
        }
        Instance = this;
        Library = (SceneLibraryData)GD.Load(sceneLibraryPath);
        if (Library == null)
        {
            GD.PushError($"Missing resource '{sceneLibraryPath}'");
            return;
        }
    }
    
    public void GoToScene(PackedScene packedScene)
    {
        void DoSwitch()
        {
            if (GetTree().CurrentScene != null && IsInstanceValid(GetTree().CurrentScene))
            {
                GetTree().CurrentScene.QueueFree();
            }
            // Instance the new scene.
            var nextScene = packedScene.Instantiate();

            // Add it to the active scene, as child of root.
            GetTree().Root.AddChild(nextScene);

            // Optionally, to make it compatible with the SceneTree.change_scene_to_file() API.
            GetTree().CurrentScene = nextScene;
            GetTree().ProcessFrame -= DoSwitch;
        }

        GetTree().ProcessFrame += DoSwitch;
    }

    public void ApplicationQuit()
    {
        var sceneTree = GetTree();
        if (sceneTree != null)
        {
            GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
            GetTree().Quit();
        }
    }
}
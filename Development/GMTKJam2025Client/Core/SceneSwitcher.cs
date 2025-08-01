using Godot;

namespace GMTKJam2025.UI;

public partial class SceneSwitcher : Node
{
    protected CanvasLayer _uiRoot;
    public Node CurrentScene { get; private set; }
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
        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
        Library = (SceneLibraryData)GD.Load(sceneLibraryPath);
        if (Library == null)
        {
            GD.PushError($"Missing resource '{sceneLibraryPath}'");
            return;
        }
    }
    
    public void GoToScene(string path)
    {
        void DoSwitch()
        {
            CurrentScene.Free();
            var nextScene = (PackedScene)GD.Load(path);

            if (nextScene != null)
            {
                CurrentScene = nextScene.Instantiate();
                GetTree().Root.AddChild(CurrentScene);
                GetTree().CurrentScene = CurrentScene;
            }
            else
            {
                GD.PushError($"Couldn't load packed scene with path '{path}'");
            }

            GetTree().ProcessFrame -= DoSwitch;
        }

        GetTree().ProcessFrame += DoSwitch;
    }

    public void GoToScene(PackedScene packedScene)
    {
        void DoSwitch()
        {
            if (CurrentScene != null)
            {
                CurrentScene.QueueFree();
            }
            // Instance the new scene.
            CurrentScene = packedScene.Instantiate();

            // Add it to the active scene, as child of root.
            GetTree().Root.AddChild(CurrentScene);

            // Optionally, to make it compatible with the SceneTree.change_scene_to_file() API.
            GetTree().CurrentScene = CurrentScene;
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
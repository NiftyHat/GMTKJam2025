using Godot;

namespace GMTKJam2025.Audio;

public partial class SoundPlayer : Node
{
    public void PlayOneShot(SoundEffectSample sample, string busName = "SFX")
    {
        AudioStreamPlayer2D player = new AudioStreamPlayer2D();
        player.SetStream(sample.Stream);
        player.VolumeDb = sample.VolumeDb;
        player.PitchScale = sample.PitchScale;
        player.Bus = busName;
        player.Finished += () => player.QueueFree();
        AddChild(player);
        player.Position = Vector2.Zero;
        player.Play();
    }
}
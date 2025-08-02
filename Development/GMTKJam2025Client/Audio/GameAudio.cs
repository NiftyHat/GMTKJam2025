using Godot;

namespace GMTKJam2025.Audio;

[GlobalClass]
public partial class GameAudio : Node
{
    [Export] private SoundEffectSample _musicNormal;
    [Export] private SoundEffectSample _musicFinal;
    [Export] private SoundEffectSample _musicInfinite;

    public void PlayRacing()
    {
        if (_musicNormal != null)
        {
            MusicPlayer.Instance.Play(_musicNormal);
        }
    }

    public void PlayFinalLap()
    {
        if (_musicFinal != null)
        {
            MusicPlayer.Instance.Play(_musicFinal);
        }
    }

    public void PlayInfiniteLaps()
    {
        if (_musicFinal != null)
        {
            MusicPlayer.Instance.Play(_musicInfinite);
        }
    }
    
}
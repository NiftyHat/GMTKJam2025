using Godot;

namespace GMTKJam2025.Audio;

[GlobalClass]
public partial class GameAudio : Node
{
    [Export] private SoundEffectSample _musicNormal;
    [Export] private SoundEffectSample _musicFinal;
    [Export] private SoundEffectSample _musicInfinite;
    [Export] private SoundEffectSample _musicNormalPause;
    [Export] private SoundEffectSample _musicFinalPause;
    [Export] private SoundEffectSample _musicInfinitePause;

    public void PlayRacing()
    {
        if (_musicNormal != null)
        {
            MusicPlayer.Instance.Play(_musicNormal);
            MusicPlayer.Instance.PlayPauseMusic(_musicNormalPause);
        }
    }

    public void PlayFinalLap()
    {
        if (_musicFinal != null)
        {
            MusicPlayer.Instance.Play(_musicFinal);
            MusicPlayer.Instance.PlayPauseMusic(_musicFinalPause);
        }
    }

    public void PlayInfiniteLaps()
    {
        if (_musicFinal != null)
        {
            MusicPlayer.Instance.Play(_musicInfinite);
            MusicPlayer.Instance.PlayPauseMusic(_musicInfinitePause);
        }
    }
    
}
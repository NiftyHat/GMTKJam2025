using GMTKJam2025.Audio;
using Godot;

namespace GMTKJam2025.MainMenu;

public partial class MainMenuMusic : Node
{
    [Export] private SoundEffectSample _musicTitle;
    [Export] private SoundEffectSample _musicSelect;
    public override void _Ready()
    {
        base._Ready();
        if (_musicTitle != null)
        {
            MusicPlayer.Instance.Play(_musicTitle);
        }
    }

    public void PlaySelectMusic()
    {
        MusicPlayer.Instance.Play(_musicSelect);
    }
}
using Godot;

namespace GMTKJam2025.Audio;

public partial class MusicPlayer : Node
{
    [Export] private AudioStreamPlayer StreamPlayerA { get; set; }
    public AudioStreamPlayer CurrentStream { get; private set; }
    
    public static MusicPlayer Instance { get; private set; }

    private Tween _fadeInTween;
    private Tween _fadeOutTween;

    public override void _Ready()
    {
        if (Instance != null)
        {
            GD.PushError("MusicPlayer already exists");
            this.QueueFree();
        }
        Instance = this;
        base._Ready();
        StreamPlayerA = new AudioStreamPlayer();
        AddChild(StreamPlayerA);
        CurrentStream = StreamPlayerA;
    }

    public void Stop(SoundEffectSample soundEffectSample = null)
    {
        if (CurrentStream == null || !CurrentStream.IsPlaying())
        {
            return;
        }
        if (soundEffectSample == null || soundEffectSample.Stream == CurrentStream.Stream)
        {
            if (_fadeInTween != null)
            {
                _fadeInTween.Kill();
            }
            if (_fadeOutTween != null && _fadeOutTween.IsRunning())
            {
                return;
            }
            _fadeOutTween = CreateTween();
            _fadeOutTween.TweenProperty(CurrentStream, "volume_db", -40, 0.5f)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Cubic);
            _fadeOutTween.TweenCallback(Callable.From(() => { CurrentStream.Stop(); }));
        }
    }

    public void Stop()
    {
        CurrentStream.Stop();
        StreamPlayerA.Stop();
    }

    public void Play(SoundEffectSample soundEffectSample)
    {
        if (soundEffectSample == null)
        {
            return;
        }

        if (soundEffectSample.Stream == null)
        {
            return;
        }

        if (CurrentStream != null && soundEffectSample.Stream == CurrentStream.Stream && CurrentStream.IsPlaying())
        {
            return;
        }

        CurrentStream.Stream = soundEffectSample.Stream;
        CurrentStream.PitchScale = soundEffectSample.PitchScale;
        CurrentStream.VolumeDb = -80;
        CurrentStream.Play();
        
        _fadeInTween = CreateTween();
        _fadeInTween.TweenProperty(CurrentStream, "volume_db", soundEffectSample.VolumeDb, 0.3f)
            .From(-80)
            .SetEase(Tween.EaseType.In)
            .SetTrans(Tween.TransitionType.Cubic);
    }
}
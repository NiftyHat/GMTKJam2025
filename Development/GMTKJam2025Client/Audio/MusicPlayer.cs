using Godot;

namespace GMTKJam2025.Audio;

public partial class MusicPlayer : Node
{
    [Export] private AudioStreamPlayer StreamPlayerA { get; set; }
    public AudioStreamPlayer CurrentStream { get; private set; }
    public AudioStreamPlayer PauseStream { get; private set; }

    
    public static MusicPlayer Instance { get; private set; }

    private Tween _fadeInTween;
    private Tween _fadeOutTween;

    private float pauseSoundVolume;

    public float currentFadeVolume;

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

        PauseStream = new AudioStreamPlayer();
        AddChild(PauseStream);

        ProcessMode = ProcessModeEnum.Always;
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

    public void PlayPauseMusic(SoundEffectSample soundEffectSample)
    {
        if (soundEffectSample == null)
        {
            return;
        }

        if (soundEffectSample.Stream == null)
        {
            return;
        }

        if (PauseStream != null && soundEffectSample.Stream == PauseStream.Stream && PauseStream.IsPlaying())
        {
            return;
        }
        
        PauseStream.Stream = soundEffectSample.Stream;
        PauseStream.PitchScale = soundEffectSample.PitchScale;
        PauseStream.VolumeLinear = 0;
        pauseSoundVolume = soundEffectSample.VolumeDb;
        PauseStream.Play();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (GetTree().Paused)
        {
            if (PauseStream != null) PauseStream.VolumeDb = pauseSoundVolume;
            if (CurrentStream != null) CurrentStream.VolumeLinear = 0;   
        }
        else
        {
            if (CurrentStream != null) CurrentStream.VolumeDb = currentFadeVolume;
            if (PauseStream != null) PauseStream.VolumeLinear = 0;
        }
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
        
        _fadeInTween.TweenProperty(this, "currentFadeVolume", soundEffectSample.VolumeDb, 0.3f)
            .From(-80)
            .SetEase(Tween.EaseType.In)
            .SetTrans(Tween.TransitionType.Cubic);
    }
}
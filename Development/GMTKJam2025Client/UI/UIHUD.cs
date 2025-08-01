using GMTKJam2025.Audio;
using Godot;

namespace GMTKJam2025.UI;

public partial class UIHUD : Control
{
    [Export] private Label _lapCountLabel;
    [Export] private AnimationPlayer _countdownAnimation;
    [Export] private AudioStreamPlayer _streamMusic;
    [Export] private HUDCountdownTimer _timeRemainingView;
    
    [Export(PropertyHint.Range, "0.01,4,or_greater")] private float _pitchIncreasePerLap = 0.05f;
    [Export(PropertyHint.Range, "0.01,4,or_greater")] private float _pitchIncreaseMax = 1.5f;

    protected Timer _timer;

    public override void _Ready()
    {
        
        base._Ready();
        PlayCountdown();
        
        if (_lapCountLabel != null)
        {
            _lapCountLabel.Visible = false;
        }
    }

    public void SetLap(int lap)
    {
        if (_lapCountLabel != null)
        {
            _lapCountLabel.Text = $"Lap {lap}";
        }
       
        if (lap == 1)
        {
            PlayRacing();
        }
    }

    public void SetMusicPitch(float newValue)
    {
        _streamMusic.PitchScale = newValue;
    }
    public void PlayCountdown()
    {
        _countdownAnimation.Play("countdown");
    }

    public void PlayRacing()
    {
        
        if (_streamMusic != null)
        {
            _streamMusic.Bus = "Music";
            _streamMusic.Play();
        }

        if (_lapCountLabel != null)
        {
            _lapCountLabel.Visible = true;
        }
    }
    
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

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (_timeRemainingView != null && _timer != null)
        {
            _timeRemainingView.SetSeconds(_timer.TimeLeft);
        }
    }

    public void SetTimer(Timer levelResetTimer)
    {
        _timer = levelResetTimer;
        _timeRemainingView.SetSeconds(_timer.TimeLeft);
    }
}
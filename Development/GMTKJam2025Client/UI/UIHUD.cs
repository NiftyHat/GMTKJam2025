using GMTKJam2025.Audio;
using Godot;

namespace GMTKJam2025.UI;

public partial class UIHUD : Control
{
    [Export] private Label _lapCountLabel;
    [Export] private AnimationPlayer _countdownAnimation;
    [Export] private HUDCountdownTimer _timeRemainingView;
    [Export] private SoundEffectSample _musicNormal;

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
    
    public void PlayCountdown()
    {
        _countdownAnimation.Play("countdown");
    }

    public void PlayRacing()
    {
        
        if (_musicNormal != null)
        {
            MusicPlayer.Instance.Play(_musicNormal);
        }

        if (_lapCountLabel != null)
        {
            _lapCountLabel.Visible = true;
        }
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
using GMTKJam2025.Audio;
using Godot;

namespace GMTKJam2025.UI;

public partial class UIHUD : Control
{
    [Export] private Label _lapCountLabel;
    [Export] private AnimationPlayer _countdownAnimation;
    [Export] private HUDCountdownTimer _timeRemainingView;
    [Export] private AnimationPlayer _ghostSpawnAnimation;
    [Export] private Control[] _ghostSpawnIcons;
    
    protected Timer _timer;
    private string infinity = "∞";

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
            _lapCountLabel.Text = $"Lap {lap}/{(lap > 7 ? infinity : "7")} ";
        }
       
        if (lap == 1)
        {
            PlayRacing();
        }

        if (lap > 1)
        {
            int ghostSpawnIconIndex = lap - 2;
            if (ghostSpawnIconIndex < _ghostSpawnIcons.Length)
            {
                var ghostSpawnIcon = _ghostSpawnIcons[ghostSpawnIconIndex];
                ghostSpawnIcon.Visible = true;
            }

            if (_ghostSpawnAnimation != null)
            {
                _ghostSpawnAnimation.Play("idle");
            }
           
        }
    }
    
    public void PlayCountdown()
    {
        _countdownAnimation.Play("countdown");
    }

    public void PlayRacing()
    {
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
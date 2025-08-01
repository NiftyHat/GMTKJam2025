using Godot;

namespace GMTKJam2025.UI;

public partial class HUDCountdownTimer : Control
{
    [Export] private Label _labelSeconds;
    [Export] private Label _labelMilliSeconds;
    [Export] private AnimationPlayer _animationPlayer;

    private double _currentTimeSeconds;

    public void SetSeconds(double seconds)
    {
        if (_currentTimeSeconds == seconds)
        {
            return;
        }
        _currentTimeSeconds = seconds;
        if (seconds < 99)
        {
            _labelSeconds.Text = seconds.ToString("N0");
        }
        else
        {
            _labelSeconds.Text = "99";
        }
        double milliSeconds = Mathf.FloorToInt((seconds % 1.0d) * 100);
        _labelMilliSeconds.Text = milliSeconds.ToString("N0");
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (_currentTimeSeconds > 10f)
        {
            _animationPlayer.Play("normal");
            return;
        }
        if (_currentTimeSeconds > 5f)
        {
            _animationPlayer.Play("low");
        }
        else
        {
            _animationPlayer.Play("critical");
        }
    }
}
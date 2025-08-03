using System;
using GMTKJam2025.Audio;
using GMTKJam2025.UI;
using Godot;

namespace GMTKJam2025.Entities;

public partial class EntityCar : GameEntity
{
    [Export] private CarController _carController;
    [Export] private UIHUD _hud;
    [Export] private GhostRecorder _ghostRecorder;
    [Export] private GameAudio _gameAudio;
    
    [Export] public double TimePerCheckpoint = 4;
    [Export] public double InitialTime = 20;
    [Export] public int LapsToWin = 7;
    
    [Export] public Timer LevelResetTimer { get; set; }
    
    private EntityCheckpoint _lastTouchedCheckpoint;
    private int _lap;
    
    public int CheckpointIndex { get; set; }

    public override void _Ready()
    {
        base._Ready();
        if (_hud != null)
        {
            _hud.SetTimer(LevelResetTimer);
            _hud.PlayCountdown();
        }

        if (LevelResetTimer != null)
        {
            LevelResetTimer.Timeout += HandleTimeout;
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (OS.HasFeature("debug") && Input.IsKeyPressed(Key.C))
        {
            _lap = 7;
            _hud.SetLap(7);
            if (LevelResetTimer != null)
            {
                LevelResetTimer.Start(10);
            }
        }
    }

    private void HandleTimeout()
    {
        LevelResetTimer.Timeout -= HandleTimeout;
        if (LevelResetTimer != null)
        {
            LevelResetTimer.Stop();
        }
        if (_lap > LapsToWin)
        {
            MusicPlayer.Instance.Stop();
            SceneSwitcher.Instance.GoToScene(SceneSwitcher.Instance.Library.GameOver);
        }
        else
        {
            GetTree().ReloadCurrentScene();
        }
    }

    public void EnterCheckpoint(EntityCheckpoint entityCheckpoint)
    {
        //if there's no checkpoint we JUST started
        entityCheckpoint.Track.SetLap(_lap);
        if (_lastTouchedCheckpoint == null && entityCheckpoint.IsStartLine())
        {
            entityCheckpoint.Track.SetActiveCheckpoint(entityCheckpoint);
            _lastTouchedCheckpoint = entityCheckpoint;
            SetLap(_lap + 1);
        }
        
        if (_lastTouchedCheckpoint != null)
        {
            
            if (entityCheckpoint.Index == _lastTouchedCheckpoint.Index + 1)
            {
                TriggerNewCheckpoint(entityCheckpoint);
            }
            else if (entityCheckpoint.Index == 0 &&
                _lastTouchedCheckpoint.Index == entityCheckpoint.Track.CheckPoints.Count - 1)
            {
                TriggerNewCheckpoint(entityCheckpoint);
                SetLap(_lap + 1);
            }
        }
    }

    private void TriggerNewCheckpoint(EntityCheckpoint entityCheckpoint)
    {
        entityCheckpoint.Track.SetActiveCheckpoint(entityCheckpoint);
        _lastTouchedCheckpoint = entityCheckpoint;
        AddTime(TimePerCheckpoint);
    }

    private void AddTime(double seconds)
    {
        if (LevelResetTimer != null)
        {
            LevelResetTimer.WaitTime = LevelResetTimer.TimeLeft + seconds;
            LevelResetTimer.Start();
        }
    }

    private void SetLap(int lap)
    {
        if (lap == _lap)
        {
            return;
        }

        _carController.ChangeVisuals(lap);

        if (_ghostRecorder != null)
        {
            _ghostRecorder.TriggerRecording();
        }
        _lap = lap;
        _hud.SetLap(_lap);
        if (_lap == 1)
        {
            _hud.PlayRacing();
            _gameAudio.PlayRacing();
            if (LevelResetTimer != null)
            {
                LevelResetTimer.Start(InitialTime);
            }
        }

        if (_lap == LapsToWin)
        {
            _gameAudio.PlayFinalLap();
        }

        if (lap > LapsToWin)
        {
            _gameAudio.PlayInfiniteLaps();
        }
    }
}
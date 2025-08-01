using System;
using GMTKJam2025.UI;
using Godot;

namespace GMTKJam2025.Entities;

public partial class EntityCar : GameEntity
{
    [Export] private CarController _carController;
    [Export] private UIHUD _hud;
    
    private EntityCheckpoint _lastTouchedCheckpoint;
    private int _lap;

    public event Action<EntityCar, int> OnChangeLap;
    
    public int CheckpointIndex { get; set; }

    public override void _Ready()
    {
        base._Ready();
        if (_hud != null)
        {
            _hud.PlayCountdown();
        }
    }
    
    public void EnterCheckpoint(EntityCheckpoint entityCheckpoint)
    {
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
                entityCheckpoint.Track.SetActiveCheckpoint(entityCheckpoint);
                _lastTouchedCheckpoint = entityCheckpoint;
            }
            else if (entityCheckpoint.Index == 0 &&
                _lastTouchedCheckpoint.Index == entityCheckpoint.Track.CheckPoints.Count - 1)
            {
                entityCheckpoint.Track.SetActiveCheckpoint(entityCheckpoint);
                _lastTouchedCheckpoint = entityCheckpoint;
                SetLap(_lap + 1);
            }
        }
    }

    private void SetLap(int lap)
    {
        if (lap == _lap)
        {
            return;
        }
        _lap = lap;
        OnChangeLap?.Invoke(this, _lap);
        _hud.SetLap(_lap);
        if (_lap == 1)
        {
            _hud.PlayRacing();
        }
    }
}
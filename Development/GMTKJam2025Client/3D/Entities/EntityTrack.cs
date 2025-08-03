using System.Collections.Generic;
using Godot;

namespace GMTKJam2025.Entities;

public partial class EntityTrack : GameEntity
{
    [Export] private EntityCheckpoint[] _checkpoints;
    public IReadOnlyList<EntityCheckpoint> CheckPoints => _checkpoints;

    public override void _Ready()
    {
        base._Ready();
        for (int i = 0; i < _checkpoints.Length; i++)
        {
            var entityCheckpoint = _checkpoints[i];
            entityCheckpoint.AddToTrack(this, i);
        }
    }

    public void SetLap(int lap)
    {
        _checkpoints[0].SetText(lap.ToString());
    }

    public void SetActiveCheckpoint(EntityCheckpoint checkpoint)
    {
        if (checkpoint == null)
        {
            return;
        }
        EntityCheckpoint previousCheckpoint = null;
        EntityCheckpoint nextCheckpoint = null;
        if (checkpoint.Index == 0)
        {
            previousCheckpoint = _checkpoints[^1];
        }
        else
        {
            previousCheckpoint = _checkpoints[checkpoint.Index - 1];
        }

        if (checkpoint.Index == _checkpoints.Length - 1)
        {
            nextCheckpoint = _checkpoints[0];
        }
        else if (checkpoint.Index < _checkpoints.Length - 1)
        {
            nextCheckpoint = _checkpoints[checkpoint.Index + 1];
        }

        if (previousCheckpoint != null)
        {
            previousCheckpoint.TriggerNormal();
        }

        if (nextCheckpoint != null)
        {
            nextCheckpoint.TriggerNext();
        }
        
        checkpoint.TriggerActive();
    }
}
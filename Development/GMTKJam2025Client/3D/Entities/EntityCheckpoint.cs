using Godot;

namespace GMTKJam2025.Entities;

[GlobalClass]
public partial class EntityCheckpoint : GameEntity
{
    [Export] private EntityDetectionZone _detectionZone;
    [Export] private AnimationPlayer _animationPlayer;
    public EntityTrack Track { get; private set; }
    public int Index { get; private set; }
    public override void _Ready()
    {
        var carDetector = new EntityDetectionZone.Detector<EntityCar>();
        carDetector.OnEnter = HandleCarEnterCheckpoint;
        _detectionZone.Set(carDetector);
        _animationPlayer.Play("idle");
        base._Ready();
    }

    private void HandleCarEnterCheckpoint(EntityCar entityCar)
    {
        GD.Print("HandleCarEnterCheckpoint");
        if (entityCar == null)
        {
            return;
        }
        entityCar.EnterCheckpoint(this);
    }

    public void TriggerActive()
    {
        GD.Print($"play active {this.Name}");
        _animationPlayer.Play("active");
    }
    
    public void TriggerNormal()
    {
        GD.Print($"play normal {this.Name}");
        _animationPlayer.Play("idle");
    }

    public void TriggerNext()
    {
        GD.Print($"play next {this.Name}");
        _animationPlayer.Play("next");
        
    }
    
    public bool IsStartLine()
    {
        if (Track == null)
        {
            return false;
        }
        return this == Track.CheckPoints[0];
    }
    
    public bool IsFinishLine(int currentCheckpointIndex)
    {
        if (Track == null)
        {
            return false;
        }

        if (currentCheckpointIndex == Track.CheckPoints.Count - 1 && IsStartLine())
        {
            return true;
        }

        return false;
    }

    public bool IsNextCheckpoint(int currentCheckpointIndex)
    {
        if (currentCheckpointIndex < Track.CheckPoints.Count)
        {
            return Index == currentCheckpointIndex + 1;
        }
        else
        {
            return IsFinishLine(currentCheckpointIndex);
        }
    }

    public void AddToTrack(EntityTrack entityTrack, int index)
    {
        Track = entityTrack;
        Index = index;
        if (Index == 0)
        {
            TriggerNext();
        }
    }


}

using System;
using System.Collections.Generic;
using Godot;

namespace GMTKJam2025.Entities;

[GlobalClass]
public partial class EntityDetectionZone : Area3D
{
    private Detector _detector;

    public bool IsEnabled { get; private set; } = true;

    [Export] public bool IsLogging { get; set; }

    public void SetEnabled(bool isEnabled)
    {
        if (IsEnabled != isEnabled)
        {
            IsEnabled = isEnabled;
        }
        //TODO - disable monitoring when enabled is false.
        Monitoring = IsEnabled;
    }

    public void Set(Detector detector)
    {
        _detector = detector;
    }

    public override void _Ready()
    {
        base._Ready();
        Monitoring = true;
        BodyEntered += HandleBodyEntered;
        BodyExited += HandleBodyExited;
    }

    public override void _Process(double delta)
    {
        _detector?.Process();
    }

    private void HandleBodyEntered(Node3D body)
    {
        if (body.IsInGroup("EntityProvider") && body.HasMethod("GetEntity"))
        {
            var gameEntity = body.Call("GetEntity").As<GameEntity>();
            if (_detector != null && _detector.CanDetect(gameEntity))
            {
                _detector.Add(gameEntity);
            }
        }
    }

    private void HandleBodyExited(Node3D body)
    {
        if (body.IsInGroup("EntityProvider") && body.HasMethod("GetEntity"))
        {
            var gameEntity = body.Call("GetEntity").As<GameEntity>();
            if (IsLogging && gameEntity != null)
            {
                GD.Print(gameEntity.Name, " HandleBodyExited");
            }

            if (_detector != null && _detector.CanDetect(gameEntity))
            {
                _detector.Remove(gameEntity);
            }
        }
    }

    public abstract class Detector
    {
        public abstract void Add(GameEntity gameEntity);
        public abstract void Remove(GameEntity gameEntity);
        public abstract void Process();
        public abstract bool CanDetect(GameEntity gameEntity);
    }

    public class Detector<TEntity> : Detector where TEntity : GameEntity
    {
        private readonly HashSet<GameEntity> _items = new();

        public Action<TEntity> OnEnter;
        public Action<TEntity> OnExit;
        public Action<TEntity> OnOverlap;

        public override void Add(GameEntity gameEntity)
        {
            _items.Add(gameEntity);
            OnEnter?.Invoke(gameEntity as TEntity);
        }

        public override void Remove(GameEntity gameEntity)
        {
            _items.Remove(gameEntity);
            OnExit?.Invoke(gameEntity as TEntity);
        }

        public bool IsDetecting()
        {
            return _items != null && _items.Count > 0;
        }

        public override void Process()
        {
            foreach (var item in _items)
            {
                OnOverlap?.Invoke(item as TEntity);
            }
        }

        public override bool CanDetect(GameEntity gameEntity)
        {
            return gameEntity is TEntity;
        }
    }
}
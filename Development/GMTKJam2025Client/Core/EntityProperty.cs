using Godot;

namespace GMTKJam2025.Entities;

[GlobalClass]
[Icon("res://World/Entities/icon-entity-property.svg")]
public partial class EntityProperty : Node
{
    public delegate void EnabledUpdated(bool enabledState);

    protected GameEntity _entity;

    private bool _isEnabled = true;

    [Export]
    public bool IsEnabled
    {
        get => _isEnabled;
        set => SetEnabled(value);
    }

    public event EnabledUpdated OnEnabledUpdate;

    public void SetEnabled(bool isEnabled)
    {
        if (_isEnabled != isEnabled)
        {
            _isEnabled = isEnabled;
            OnEnabledUpdate?.Invoke(isEnabled);
        }
    }

    public override void _Ready()
    {
        _entity = GetParent<GameEntity>();
        base._Ready();
    }
}
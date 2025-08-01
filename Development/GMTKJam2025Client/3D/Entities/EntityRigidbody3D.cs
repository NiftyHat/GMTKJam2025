using Godot;

namespace GMTKJam2025.Entities;

public partial class EntityRigidbody3D : RigidBody3D, IEntityProvider
{
    [Export] private GameEntity Entity { get; set; }
	
    public GameEntity GetEntity()
    {
        return Entity;
    }
    
    public override void _Ready()
    {
        Entity ??= GetFirstEntityChild();
        if (Entity != null && !IsInGroup("EntityProvider"))
        {
            AddToGroup("EntityProvider");
        }
        base._Ready();
    }
    
    private GameEntity GetFirstEntityChild()
    {
        int childCount = GetChildCount(true);
        for (var i = 0; i < childCount; i++)
        {
            var child = GetChildOrNull<GameEntity>(i, true);
            if (child != null)
            {
                return child;
            }
        }
        return null;
    }
    
}
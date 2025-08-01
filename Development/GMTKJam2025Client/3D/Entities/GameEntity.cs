using GMTKJam2025.Core;
using Godot;

namespace GMTKJam2025.Entities;

[GlobalClass]
public partial class GameEntity : Node3D
{
    private readonly TypedStorage<EntityProperty> _properties = new();

    public GameEntity GetEntity()
    {
        return this;
    }

    public TProp GetProperty<TProp>() where TProp : EntityProperty
    {
        return _properties.Get<TProp>();
    }

    public bool TryGetProperty<TProp>(out TProp property) where TProp : EntityProperty
    {
        return _properties.TryGet(out property);
    }

    public static bool TryGetProperty<TProp>(Node3D node3D, out TProp property, out GameEntity gameEntity)
        where TProp : EntityProperty
    {
        var isInGroup = node3D.IsInGroup("EntityProvider");
        var hasMethod = node3D.HasMethod("GetEntity");
        if (isInGroup && hasMethod)
        {
            gameEntity = node3D.Call("GetEntity").As<GameEntity>();
            if (gameEntity != null)
            {
                return gameEntity.TryGetProperty(out property);
            }
        }

        gameEntity = null;
        property = null;
        return false;
    }

    public static bool TryGetEntity<[MustBeVariant] TEntity>(Node node, out TEntity gameEntity) where TEntity : GameEntity
    {
        var isInGroup = node.IsInGroup("EntityProvider");
        var hasMethod = node.HasMethod("GetEntity");
        if (isInGroup && hasMethod)
        {
            gameEntity = node.Call("GetEntity").As<TEntity>();
            return true;
        }
        gameEntity = null;
        return false;
    }

    public static bool TryGetProperty<TProperty>(Node node, out TProperty property) where TProperty : EntityProperty
    {
        var isInGroup = node.IsInGroup("EntityProvider");
        var hasMethod = node.HasMethod("GetEntity");
        if (isInGroup && hasMethod)
        {
            var gameEntity = node.Call("GetEntity").As<GameEntity>();
            if (gameEntity != null)
            {
                return gameEntity.TryGetProperty(out property);
            }
        }
        property = null;
        return false;
    }

    public void AddProperty<TPropType>(TPropType property) where TPropType : EntityProperty
    {
        _properties.Add(property);
    }
}
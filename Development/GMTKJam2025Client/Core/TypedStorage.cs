using System;
using System.Collections.Generic;
using Godot;

namespace GMTKJam2025.Core;

public class TypedStorage<TBaseType>
{
    public Dictionary<Type, TBaseType> _map = new();

    public void Add<TType>(TType item) where TType : TBaseType
    {
        var key = typeof(TType);
        if (_map.TryGetValue(key, out var storedItem))
            if (!storedItem.Equals(item))
            {
                GD.PushError(
                    $"{nameof(TypedStorage<TBaseType>)} can't add item {item} to key {key.Name} since it's already populated by {storedItem}");
                return;
            }

        _map.Add(key, item);
    }

    public TType Get<TType>() where TType : TBaseType
    {
        var key = typeof(TType);
        if (_map.TryGetValue(key, out var storedItem)) return (TType)storedItem;
        return default;
    }

    public bool TryGet<TType>(out TType item) where TType : TBaseType
    {
        var key = typeof(TType);
        if (_map.TryGetValue(key, out var storedItem))
        {
            item = (TType)storedItem;
            return true;
        }

        item = default;
        return false;
    }

    public void Remove<TType>() where TType : TBaseType
    {
        var key = typeof(TType);
        _map.Remove(key);
    }
}
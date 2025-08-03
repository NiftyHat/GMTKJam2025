using Godot;

namespace GMTKJam2025.Entities;

public partial class EntityAnimationTrigger : Node3D
{
    [Export] private AnimationPlayer _animationPlayer;

    public void PlayAnimation(string animationName)
    {
        _animationPlayer.Play(animationName);
    }
}
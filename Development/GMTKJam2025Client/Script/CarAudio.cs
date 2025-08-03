using System;
using Godot;

namespace GMTKJam2025;

public partial class CarAudio : Node3D
{
    [Export] public AudioStreamPlayer3D StreamPlayerEngine { get; set; }
    [Export] public AudioStreamPlayer3D StreamPlayerCollider { get; set; }

    protected float _audibleVelocity = 0;
    protected float _maxAudibleSpeed = 100f;
    protected Vector3 _linearVelocity;
    protected float _velocity;
    protected int _gear;
    protected float _speedInput;
    
    public void UpdateEngineSound(Vector3 linearVelocity, float speedInput)
    {
        _linearVelocity = linearVelocity;
        _velocity = linearVelocity.Abs().Length();
        _speedInput = speedInput;
    }

    public void PlayCollisionSound(Vector3 linearVelocity)
    {
        StreamPlayerCollider.Play();
    }
    
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (_velocity <= 2.0f)
        {
            _audibleVelocity = 0;
        }

        if (_speedInput > 0)
        {
            if (_audibleVelocity < _velocity)
            {
                _audibleVelocity = Mathf.Lerp(_audibleVelocity, _velocity, 0.9f * (float)delta);
            }
            if (_audibleVelocity > _velocity)
            {
                _audibleVelocity -= 1f;
            }
        }
        else
        {
            _audibleVelocity -= 1f;
        }

        if (_audibleVelocity < 0)
        {
            _audibleVelocity = 0;
        }


        int newGear = (int)(_audibleVelocity / 10f);
        if (_gear != newGear)
        {
            _gear = newGear;
            StreamPlayerEngine.PitchScale = 0.9f + _gear * 0.1f;
            StreamPlayerEngine.Play();
        }
    }
}
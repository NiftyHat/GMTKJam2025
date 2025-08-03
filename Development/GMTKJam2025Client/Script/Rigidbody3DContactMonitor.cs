using Godot;

namespace GMTKJam2025;

[GlobalClass]
public partial class Rigidbody3DContactMonitor : Node3D
{
    [Export] private RigidBody3D _rb;
    [Export] private float MinimumContactImpulse;

    private Vector3 _lastGlobalPosition;
    private Vector3 _lastLinearVelocity;
    private Vector3 _lastAngularVelocity;
    private float _minimumContactImpulseSquared;

    public delegate void OnBodyContact(Vector3 impulse, GodotObject godotObject);

    public OnBodyContact OnContact;

    public override void _Ready()
    {
        _rb = GetParent<RigidBody3D>();
        if (_rb == null)
        {
            GD.PushError("Rigidbody3DContactMonitor has no rigibody");
            return;
        }

       
        _minimumContactImpulseSquared = MinimumContactImpulse * MinimumContactImpulse;
        _rb.ContactMonitor = true;
        _rb.MaxContactsReported = 5;
        _rb.BodyEntered += HandleBodyEntered;
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateCachedVectors();
    }

    void UpdateCachedVectors()
    {
        _lastGlobalPosition = _rb.GlobalPosition;
        _lastLinearVelocity = _rb.LinearVelocity;
        _lastAngularVelocity = _rb.AngularVelocity;
    }

    public static Vector3 GetPointVelocity(Vector3 point, Vector3 linearVelocity, Vector3 angularVelocity,
        Vector3 relativeBodyGlobalPosition)
    {
        return linearVelocity + angularVelocity.Cross(point - relativeBodyGlobalPosition);
    }

    public void HandleBodyEntered(Node body)
    {
        if (_rb == null)
        {
            return;
        }
        
        PhysicsDirectBodyState3D state = PhysicsServer3D.BodyGetDirectState(_rb.GetRid());

        for (int i = 0; i < state.GetContactCount(); i++)
        {
            Vector3 contactPt = state.GetContactColliderPosition(i);

            Vector3 currentPtVelocity =
                GetPointVelocity(contactPt, _rb.LinearVelocity, _rb.AngularVelocity, _rb.Position);

            Vector3 lastPtVelocity =
                GetPointVelocity(contactPt, _lastLinearVelocity, _lastAngularVelocity, _rb.Position);

            Vector3 impulse = currentPtVelocity - lastPtVelocity;
            // ^ this isn't a very detailed approach, and it doesn't account for like, delta or mass or anything, but you could add that if needed. in my use-case i just need to know relatively how hard something was hit

            if (impulse.LengthSquared() < _minimumContactImpulseSquared)
            {
                continue;
            }
            // implicit else...


            // now you can handle the collision however you want...

            GodotObject contactObject = state.GetContactColliderObject(i);
            OnContact(impulse, contactObject);
        }
    }
}
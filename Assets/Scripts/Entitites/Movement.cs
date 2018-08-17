using UnityEngine;

public class Movement {

    protected Rigidbody Body;

    protected Transform Transform;

    public float Speed { get; protected set; }

    public float Distance { get; protected set; }

    public Vector3 LastPosition { get; protected set; }

    public Movement(Rigidbody body, Transform transform)
    {
        Body      = body;
        Transform = transform;

        LastPosition = Transform.position;
        Speed        = 0f;
        Distance     = 0f;
    }

    public void Update()
    {
        float difference = Vector3.Distance(LastPosition, Transform.position);

        Speed = difference / Time.fixedDeltaTime;

        Distance += difference;

        LastPosition = Transform.position;
    }

    public void Move(Vector3 velocity)
    {
        Body.AddForce(velocity, ForceMode.Force);
    }
}

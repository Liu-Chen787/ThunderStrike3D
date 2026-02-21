using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DropItemFall : MonoBehaviour
{
    public float fallSpeed = 1.5f;
    public float driftX = 0f;
    public float zPlane = 0f;      
    public float lifeTime = 12f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true; // kinematic + MovePosition 最稳定
    }

    void OnEnable()
    {
        var p = transform.position;
        p.z = zPlane;
        transform.position = p;

        if (lifeTime > 0f) Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        Vector3 p = rb.position;
        p.x += driftX * Time.fixedDeltaTime;
        p.y -= fallSpeed * Time.fixedDeltaTime;
        p.z = zPlane;
        rb.MovePosition(p);
    }
}

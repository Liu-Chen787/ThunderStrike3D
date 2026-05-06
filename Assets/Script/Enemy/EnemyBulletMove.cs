using UnityEngine;

public class EnemyBulletMove : MonoBehaviour
{
    [Header("Move")]
    public float speed = 18f;
    public int directionSign = -1;

    [Header("Damage")]
    public int damage = 1;  // Inspector 直接修改

    [Header("Auto Destroy")]
    public float maxLifeTime = 6f;

    float _life;

    void OnEnable()
    {
        _life = 0f;

        var rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void Update()
    {
        transform.position += transform.forward * (speed * directionSign) * Time.deltaTime;

        _life += Time.deltaTime;
        if (_life >= maxLifeTime) Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        Destroy(gameObject);
    }
}
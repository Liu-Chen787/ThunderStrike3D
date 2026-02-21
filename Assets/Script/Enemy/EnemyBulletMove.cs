using UnityEngine;

public class EnemyBulletMove : MonoBehaviour
{
    [Header("Move")]
    public float speed = 18f;
    public int directionSign = -1; // -1 = 往世界 -Z；+1 = 往世界 +Z（不对就改这个）

    [Header("Auto Destroy")]
    public float maxLifeTime = 6f; // 保险上限

    float _life;

    void OnEnable()
    {
        _life = 0f;

        // 强制关闭重力（很多“掉落”就是这里）
        var rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero; // Unity6 用 linearVelocity
            rb.angularVelocity = Vector3.zero;
        }
    }

    void Update()
    {
        // 沿自身Z轴飞：transform.forward
        transform.position += transform.forward * (speed * directionSign) * Time.deltaTime;

        _life += Time.deltaTime;
        if (_life >= maxLifeTime) Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        other.GetComponent<PlayerHealth>()?.TakeDamage(1);
        Destroy(gameObject);
    }

}

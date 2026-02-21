using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float speed = 30f;
    public float lifeTime = 3f;

    float _t;

    void OnEnable()
    {
        _t = 0f;
    }

    void Update()
    {
        // 沿自身 forward（Z轴）飞 —— 方向由 Instantiate 的 rotation 决定
        transform.position += transform.forward * speed * Time.deltaTime;

        _t += Time.deltaTime;
        if (_t >= lifeTime)
            Destroy(gameObject);
    }
}

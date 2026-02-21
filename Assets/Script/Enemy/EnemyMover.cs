using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [Header("Bounds")]
    public float minX = -13f;
    public float maxX = 13f;
    public float minY = -15f;
    public float maxY = 2f;
    public float zPlane = 8f;

    [Header("Move")]
    public float speed = 3.5f;

    Vector3 _dir;

    void Start()
    {
        // 随机初始方向（保证不会是0向量）
        _dir = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            0f
        ).normalized;

        if (_dir == Vector3.zero)
            _dir = Vector3.down;

        // 固定Z轴
        Vector3 p = transform.position;
        p.z = zPlane;
        transform.position = p;
    }

    void Update()
    {
        // 只在XY移动（Z方向永远是0）
        transform.position += _dir * speed * Time.deltaTime;

        Vector3 p = transform.position;
        bool bounced = false;

        // X 边界反弹
        if (p.x < minX)
        {
            p.x = minX;
            _dir.x *= -1f;
            bounced = true;
        }
        else if (p.x > maxX)
        {
            p.x = maxX;
            _dir.x *= -1f;
            bounced = true;
        }

        // Y 边界反弹
        if (p.y < minY)
        {
            p.y = minY;
            _dir.y *= -1f;
            bounced = true;
        }
        else if (p.y > maxY)
        {
            p.y = maxY;
            _dir.y *= -1f;
            bounced = true;
        }

        // 强制固定Z（防止任何东西改Z）
        p.z = zPlane;
        transform.position = p;

        // 反弹后轻微扰动，让运动更自然
        if (bounced)
        {
            _dir = (_dir + new Vector3(
                Random.Range(-0.2f, 0.2f),
                Random.Range(-0.2f, 0.2f),
                0f
            )).normalized;

            if (_dir == Vector3.zero)
                _dir = Vector3.down;
        }
    }
}

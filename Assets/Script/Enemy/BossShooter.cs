using UnityEngine;

public class BossShooter : MonoBehaviour
{
    [Header("Bullet preform")]
    public GameObject bulletPrefab;
    public Transform  firePoint;

    [Header("Phase 1 - 3 to the sector shape")]
    public float p1Interval  = 1.8f;
    public float p1SpreadDeg = 20f;

    [Header("Phase 2 - 5 to the sector shape")]
    public float p2Interval  = 1.2f;
    public float p2SpreadDeg = 40f;

    [Header("Phase 3 - Full-directional bullet hell")]
    public float p3Interval    = 0.5f;
    public int   p3BulletCount = 12;

    [Header("Phase 3 - Burst firing every X seconds")]
    public float burstInterval    = 3f;
    public int   burstBulletCount = 5;
    public float burstSpreadDeg   = 15f;

    [Header("Bullet movement")]
    public float forwardSpeed = 18f;   // -Z 方向（朝玩家深度）
    public float spreadSpeed  = 4f;    // XY 侧移扩散

    float      _timer;
    float      _burstTimer;
    int        _lastPhase = 1;
    BossHealth _health;

    void Awake()
    {
        _health = GetComponent<BossHealth>();
    }

    void Update()
    {
        if (!bulletPrefab || !firePoint) return;
        if (_health == null) return;

        int phase = _health.CurrentPhase;

        if (phase != _lastPhase)
        {
            Debug.Log($"[BossShooter] Phase: {_lastPhase} → {phase}");
            _lastPhase  = phase;
            _timer      = 0f;
            _burstTimer = 0f;
        }

        float interval = phase switch
        {
            2 => p2Interval,
            3 => p3Interval,
            _ => p1Interval
        };

        _timer += Time.deltaTime;
        if (_timer >= interval)
        {
            _timer = 0f;
            Fire(phase);
        }

        if (phase == 3)
        {
            _burstTimer += Time.deltaTime;
            if (_burstTimer >= burstInterval)
            {
                _burstTimer = 0f;
                FireSpread(burstBulletCount, burstSpreadDeg);
            }
        }
    }

    void Fire(int phase)
    {
        switch (phase)
        {
            case 1: FireSpread(3,              p1SpreadDeg); break;
            case 2: FireSpread(5,              p2SpreadDeg); break;
            case 3: FireCircleFull(p3BulletCount);           break;
        }
    }

    // 扇形：以正前方（-Z）为主方向，XY 侧移扩散
    void FireSpread(int count, float totalSpread)
    {
        float step  = count > 1 ? totalSpread / (count - 1) : 0f;
        float start = -totalSpread / 2f;

        for (int i = 0; i < count; i++)
        {
            float rad = (start + step * i) * Mathf.Deg2Rad;
            Vector2 xy = new Vector2(Mathf.Sin(rad), -Mathf.Cos(rad));
            SpawnBullet(xy);
        }
    }

    // 全向 360 度
    void FireCircleFull(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float a = 360f / count * i * Mathf.Deg2Rad;
            SpawnBullet(new Vector2(Mathf.Cos(a), Mathf.Sin(a)));
        }
    }

    void SpawnBullet(Vector2 xyDir)
    {
        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // 不 disable EnemyBulletMove（保留 OnTriggerEnter 伤害检测）
        // 但把 speed 设为 0，让 Rigidbody 接管移动
        var mover = b.GetComponent<EnemyBulletMove>();
        if (mover != null)
            mover.speed = 0f;

        var rb = b.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic    = false;
            rb.useGravity     = false;
            rb.linearVelocity = new Vector3(
                xyDir.x * spreadSpeed,    // X 侧移
                xyDir.y * spreadSpeed,    // Y 侧移
                -forwardSpeed             // -Z = 朝玩家飞
            );
        }

        Destroy(b, 6f);
    }
}
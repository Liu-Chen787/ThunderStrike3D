using UnityEngine;

/// <summary>
/// 三态状态机 AI：Patrol → Chase → Attack → Patrol
/// 替换 EnemyMover.cs，直接挂在敌人预制件根节点上
/// </summary>
public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack }

    [Header("Detection")]
    public float detectRange  = 8f;   // 进入 Chase 的距离
    public float attackRange  = 4f;   // 进入 Attack 的距离
    public float loseRange    = 12f;  // 超过此距离返回 Patrol

    [Header("Movement")]
    public float patrolSpeed  = 3.5f;
    public float chaseSpeed   = 5.5f;
    public float zPlane       = 8f;

    [Header("Bounds (Patrol 反弹)")]
    public float minX = -13f, maxX = 13f;
    public float minY = -15f, maxY =  2f;

    [Header("Attack Fire Rate Multiplier")]
    [Tooltip("Attack 状态下射击间隔乘以此值（<1 = 更快）")]
    public float attackFireMultiplier = 0.4f;

    // ── 内部 ──────────────────────────────────────────
    State        _state  = State.Patrol;
    Vector3      _patrolDir;
    Transform    _player;
    EnemyShooter3D _shooter;

    // 用于记录 Shooter 原始间隔
    float        _baseFireInterval;

    // ── 生命周期 ──────────────────────────────────────
    void Awake()
    {
        _shooter = GetComponentInChildren<EnemyShooter3D>();
        if (_shooter != null) _baseFireInterval = _shooter.fireInterval;
    }

    void Start()
    {
        // 固定 Z 轴
        Vector3 p = transform.position;
        p.z = zPlane;
        transform.position = p;

        // 随机初始巡逻方向
        _patrolDir = RandomDir();

        // 找玩家（只找一次，性能比每帧 Find 好）
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) _player = go.transform;
    }

    void Update()
    {
        if (_player == null) return;

        float dist = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.y),
            new Vector2(_player.position.x,   _player.position.y));

        // ── 状态转移 ─────────────────────────────────
        switch (_state)
        {
            case State.Patrol:
                if (dist <= detectRange) EnterChase();
                break;

            case State.Chase:
                if (dist <= attackRange)  EnterAttack();
                else if (dist >= loseRange) EnterPatrol();
                break;

            case State.Attack:
                if (dist > attackRange) EnterChase();
                break;
        }

        // ── 状态行为 ─────────────────────────────────
        switch (_state)
        {
            case State.Patrol: DoPatrol(); break;
            case State.Chase:  DoChase();  break;
            case State.Attack: DoAttack(); break;
        }

        // 强制锁 Z 轴
        Vector3 pos = transform.position;
        pos.z = zPlane;
        transform.position = pos;
    }

    // ── 状态进入 ──────────────────────────────────────
    void EnterPatrol()
    {
        _state = State.Patrol;
        _patrolDir = RandomDir();
        SetFireInterval(_baseFireInterval);   // 恢复正常射速
    }

    void EnterChase()
    {
        _state = State.Chase;
        SetFireInterval(_baseFireInterval);
    }

    void EnterAttack()
    {
        _state = State.Attack;
        // 进入攻击状态：射速加快
        SetFireInterval(_baseFireInterval * attackFireMultiplier);
    }

    // ── 状态行为 ──────────────────────────────────────
    void DoPatrol()
    {
        transform.position += _patrolDir * patrolSpeed * Time.deltaTime;

        Vector3 p = transform.position;
        bool bounced = false;

        if (p.x < minX) { p.x = minX; _patrolDir.x =  Mathf.Abs(_patrolDir.x); bounced = true; }
        if (p.x > maxX) { p.x = maxX; _patrolDir.x = -Mathf.Abs(_patrolDir.x); bounced = true; }
        if (p.y < minY) { p.y = minY; _patrolDir.y =  Mathf.Abs(_patrolDir.y); bounced = true; }
        if (p.y > maxY) { p.y = maxY; _patrolDir.y = -Mathf.Abs(_patrolDir.y); bounced = true; }

        transform.position = p;

        if (bounced)
            _patrolDir = (_patrolDir + new Vector3(
                Random.Range(-0.3f, 0.3f),
                Random.Range(-0.3f, 0.3f), 0f)).normalized;
    }

    void DoChase()
    {
        // 只追踪 XY，Z 不变
        Vector3 dir = new Vector3(
            _player.position.x - transform.position.x,
            _player.position.y - transform.position.y, 0f).normalized;

        transform.position += dir * chaseSpeed * Time.deltaTime;
    }

    void DoAttack()
    {
        // Attack 状态停止移动，朝向玩家
        // 只绕 Z 轴旋转（遵守原有架构规范）
        Vector3 diff = _player.position - transform.position;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // ── 工具 ─────────────────────────────────────────
    Vector3 RandomDir()
    {
        Vector3 d = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f), 0f).normalized;
        return d == Vector3.zero ? Vector3.down : d;
    }

    void SetFireInterval(float interval)
    {
        if (_shooter != null)
            _shooter.fireInterval = interval;
    }

    // ── Gizmos（Editor 可视化检测范围）──────────────
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, loseRange);
    }
#endif
}
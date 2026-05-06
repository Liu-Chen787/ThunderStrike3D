using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("")]
    public float minY   =  1f;    // EnemyMover.maxY=2 附近，Boss 在上半区
    public float maxY   =  7f;
    public float minX   = -10f;   // EnemyMover: ±13，Boss 略小
    public float maxX   =  10f;
    public float fixedZ =  8f;

    [Header("Movement speed at each stage")]
    public float speedPhase1 = 2.5f;
    public float speedPhase2 = 4f;
    public float speedPhase3 = 6f;

    [Header("Random target interval (seconds)")]
    public float minWanderTime = 1.0f;
    public float maxWanderTime = 2.5f;

    [Header("Phase 3 - Meteorite Summoning")]
    public AsteroidSpawner asteroidSpawner;
    public float phase3AsteroidInterval = 2f;

    Vector3    _targetPos;
    float      _wanderTimer;
    int        _lastPhase = 1;
    BossHealth _health;

    void Awake()
    {
        _health = GetComponent<BossHealth>();
    }

    void Start()
    {
        Vector3 p = transform.position;
        p.z = fixedZ;
        p.y = Mathf.Clamp(p.y, minY, maxY);
        transform.position = p;

        if (asteroidSpawner != null)
        {
            asteroidSpawner.enabled = false;
            Debug.Log($"[BossAI] Asteroid spawner found: {asteroidSpawner.name}");
        }
        else
            Debug.LogError("[BossAI] asteroidSpawner is NULL — drag AsteroidsSpawner into Inspector!");

        PickNewTarget();
    }

    void Update()
    {
        int phase = _health != null ? _health.CurrentPhase : 1;

        if (phase != _lastPhase)
        {
            _lastPhase = phase;
            HandlePhaseChange(phase);
        }

        float speed = phase switch
        {
            2 => speedPhase2,
            3 => speedPhase3,
            _ => speedPhase1
        };

        transform.position = Vector3.MoveTowards(
            transform.position, _targetPos, speed * Time.deltaTime);

        _wanderTimer -= Time.deltaTime;
        if (_wanderTimer <= 0f || Vector3.Distance(transform.position, _targetPos) < 0.2f)
            PickNewTarget();

        // 强制锁 Z
        Vector3 pos = transform.position;
        pos.z = fixedZ;
        transform.position = pos;
    }

    void PickNewTarget()
    {
        _targetPos   = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            fixedZ);
        _wanderTimer = Random.Range(minWanderTime, maxWanderTime);
    }

    void HandlePhaseChange(int phase)
    {
        Debug.Log($"[BossAI] Phase changed to {phase}");

        if (asteroidSpawner == null)
        {
            Debug.LogError("[BossAI] asteroidSpawner is NULL!");
            return;
        }

        if (phase == 3)
        {
            asteroidSpawner.spawnInterval = phase3AsteroidInterval;
            asteroidSpawner.enabled       = true;
            Debug.Log("[BossAI] Asteroid spawner ENABLED");
        }
        else
        {
            asteroidSpawner.enabled = false;
            Debug.Log("[BossAI] Asteroid spawner DISABLED");
        }
    }
}
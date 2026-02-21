using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Type")]
    public bool isElite = false;

    [Header("Health")]
    public int maxHP = 5;

    [Header("Score")]
    public int scoreOnKill = 10;

    [Header("VFX")]
    public GameObject explosionPrefab;

    [Header("Drop (Elite recommended)")]
    public DropOnDeath dropOnDeath;   // 可选：精英挂DropOnDeath，普通不挂

    int _hp;
    Transform _root;
    bool _dead;

    void Awake()
    {
        _hp = maxHP;
        _root = transform.root;

        // 如果你没手动拖 dropOnDeath，就自动找
        if (dropOnDeath == null)
            dropOnDeath = _root.GetComponent<DropOnDeath>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (_dead) return;
        if (!other.CompareTag("PlayerBullet")) return;

        _hp--;
        Destroy(other.gameObject);

        // 受击闪白（如果你挂了 DamageFlash）
        var flash = _root.GetComponent<DamageFlash>();
        if (flash != null) flash.Flash();

        if (_hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (_dead) return;
        _dead = true;

        SpawnExplosion();

        // 掉落（精英怪建议 dropChance 高一点）
        if (dropOnDeath != null)
            dropOnDeath.TryDrop(_root.position);

        // 计分 + 击杀计数（新GameManager）
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreOnKill);
            GameManager.Instance.AddKill(isElite);
        }

        // 通知 spawner：精英死亡，减少alive计数
        if (isElite)
        {
            var spawner = FindFirstObjectByType<EnemySpawnerLimited>();
            if (spawner != null) spawner.NotifyEliteDead();
        }

        Destroy(_root.gameObject);
    }

    void SpawnExplosion()
    {
        if (!explosionPrefab) return;

        Vector3 pos = _root.position;

        // 用可视中心，避免模型偏移造成爆炸错位
        var r = _root.GetComponentInChildren<Renderer>();
        if (r != null) pos = r.bounds.center;

        Instantiate(explosionPrefab, pos, Quaternion.identity);
    }
}

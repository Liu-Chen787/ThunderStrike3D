using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BossHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHP = 300;
    int _hp;

    [Header("Phase Thresholds")]
    public float phase2Threshold = 0.6f;
    public float phase3Threshold = 0.3f;

    [Header("VFX")]
    public GameObject explosionPrefab;

    [Header("Drop System")]
    public GameObject heartPrefab;
    public GameObject lightningPrefab;
    [Range(0f, 1f)]
    public float dropChance = 0.05f;  // 5%（测试完成后可改回1%）
    public float dropZ      = 0f;     // 与玩家同一 Z 平面

    public int   CurrentPhase { get; private set; } = 1;
    public float HPRatio      => (float)_hp / maxHP;

    public event Action<int> OnPhaseChanged;
    public event Action      OnBossDead;

    bool _dead;

    void Awake()
    {
        _hp = maxHP;
    }

    void OnTriggerEnter(Collider other)
    {
        if (_dead) return;
        if (!other.CompareTag("PlayerBullet")) return;

        Destroy(other.gameObject);
        GetComponent<DamageFlash>()?.Flash();

        _hp--;
        if (_hp < 0) _hp = 0;

        Debug.Log($"[Boss] Hit! HP={_hp}/{maxHP}  Phase={CurrentPhase}");

        BossHUDUI.Instance?.UpdateHP(HPRatio, CurrentPhase);

        TryDrop();
        CheckPhase();

        if (_hp <= 0) Die();
    }

    void TryDrop()
    {
        if (Random.value > dropChance) return;

        GameObject prefab = Random.value < 0.5f ? heartPrefab : lightningPrefab;
        if (prefab == null)
        {
            Debug.LogWarning("[Boss] Drop prefab is null! Assign heartPrefab / lightningPrefab in Inspector.");
            return;
        }

        // 参考 DropOnDeath.TryDrop：XY 偏移 + 强制 Z = 玩家平面
        Vector3 pos = transform.position;
        pos += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.2f, 0.2f), 0f);
        pos.z = dropZ;

        Instantiate(prefab, pos, Quaternion.identity);
        Debug.Log($"[Boss] Dropped {prefab.name} at {pos}");
    }

    void CheckPhase()
    {
        int newPhase = 1;
        if      (HPRatio <= phase3Threshold) newPhase = 3;
        else if (HPRatio <= phase2Threshold) newPhase = 2;

        if (newPhase != CurrentPhase)
        {
            CurrentPhase = newPhase;
            OnPhaseChanged?.Invoke(CurrentPhase);
            Debug.Log($"[Boss] Phase → {CurrentPhase}");
        }
    }

    void Die()
    {
        if (_dead) return;
        _dead = true;

        if (explosionPrefab)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        OnBossDead?.Invoke();
        GameManager.Instance?.ShowVictoryFromBoss();
        Destroy(gameObject, 0.1f);
    }
}
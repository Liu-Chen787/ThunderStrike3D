using UnityEngine;

public class EnemySpawnerLimited : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject enemyPrefab;      // 普通敌机
    public GameObject elitePrefab;      // 精英敌机（EnemyPro）

    [Header("Spawn")]
    public float spawnInterval = 1f;

    [Header("Spawn Range")]
    public float minX = -13f;
    public float maxX = 13f;
    public float minY = -15f;
    public float maxY = 2f;
    public float spawnZ = 8f;

    [Header("Limit")]
    public int maxTotalSpawn = 20;      // 总共最多生成多少“普通敌机”（不含精英/或你也可以把精英算进去）
    public int maxEliteAlive = 1;       // 场上精英最多几个

    float _timer;
    int _spawnedTotal;
    int _eliteAlive;

    void Update()
    {
        // 普通敌机按间隔刷到上限
        if (_spawnedTotal < maxTotalSpawn)
        {
            _timer += Time.deltaTime;
            if (_timer >= spawnInterval)
            {
                _timer = 0f;
                SpawnNormal();
            }
        }
    }

    Vector3 GetSpawnPos()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        return new Vector3(x, y, spawnZ);
    }

    void SpawnNormal()
    {
        if (!enemyPrefab) return;
        Instantiate(enemyPrefab, GetSpawnPos(), Quaternion.identity);
        _spawnedTotal++;
    }

    // 给 GameManager 调用：规则A 触发时刷精英
    public void TrySpawnElite()
    {
        if (!elitePrefab) return;
        if (_eliteAlive >= maxEliteAlive) return;

        Instantiate(elitePrefab, GetSpawnPos(), Quaternion.identity);
        _eliteAlive++;
    }

    // 精英死亡时由 EnemyHealth 调用
    public void NotifyEliteDead()
    {
        _eliteAlive--;
        if (_eliteAlive < 0) _eliteAlive = 0;
    }
}

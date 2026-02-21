using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public Transform asteroidsRoot;

    public float spawnInterval = 10f;
    public float spawnY = 8f;
    public Vector2 spawnXRange = new Vector2(-6f, 6f);
    public float fixedZ = 0f;

    float _t;

    void Update()
    {
        if (asteroidPrefab == null) return;

        _t += Time.deltaTime;
        if (_t >= spawnInterval)
        {
            _t = 0f;
            float x = Random.Range(spawnXRange.x, spawnXRange.y);
            Vector3 pos = new Vector3(x, spawnY, fixedZ);
            Instantiate(asteroidPrefab, pos, Quaternion.identity, asteroidsRoot);
        }
    }
}

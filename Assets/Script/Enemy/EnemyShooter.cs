using UnityEngine;

public class EnemyShooter3D : MonoBehaviour
{
    public GameObject enemyBulletPrefab;
    public float fireInterval = 1.5f;

    float _t;

    void Update()
    {
        if (!enemyBulletPrefab) return;

        _t += Time.deltaTime;
        if (_t >= fireInterval)
        {
            _t = 0f;
            Instantiate(enemyBulletPrefab, transform.position, transform.rotation);
        }
    }
}

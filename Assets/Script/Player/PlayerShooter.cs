using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float fireRate = 10f; // 每秒多少发
    float _nextFireTime;

    [Header("Audio")]
    public AudioClip shootClip;   // 射击音效

    void Update()
    {
        if (firePoint == null || bulletPrefab == null) return;

        if (Time.time >= _nextFireTime)
        {
            Shoot();
            _nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Shoot()
    {
        // 生成子弹
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 播放音效
        if (shootClip != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(shootClip);
        }
    }
}

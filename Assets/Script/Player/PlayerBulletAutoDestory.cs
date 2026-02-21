using UnityEngine;

public class PlayerBulletAutoDestroy : MonoBehaviour
{
    public float lifeTime = 3f;   // 子弹存在时间

    void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(DestroySelf), lifeTime);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}

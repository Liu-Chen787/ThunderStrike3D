using UnityEngine;

public class DamageOnTrigger : MonoBehaviour
{
    public string targetTag = "Player";
    public int damage = 10;
    public bool destroySelfOnHit = true;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        var ph = other.GetComponentInParent<PlayerHealth>();
        if (ph != null)
            ph.TakeDamage(damage);

        if (destroySelfOnHit)
            Destroy(gameObject);
    }
}

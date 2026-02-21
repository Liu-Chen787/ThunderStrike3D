using UnityEngine;

public class HitImpactVFX : MonoBehaviour
{
    public GameObject hitVfxPrefab;

    public void Spawn(Vector3 position)
    {
        if (!hitVfxPrefab) return;
        Instantiate(hitVfxPrefab, position, Quaternion.identity);
    }
}

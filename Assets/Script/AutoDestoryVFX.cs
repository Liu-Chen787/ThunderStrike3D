using UnityEngine;

public class AutoDestroyVFX : MonoBehaviour
{
    public float life = 1f;

    void Start()
    {
        Destroy(gameObject, life);
    }
}

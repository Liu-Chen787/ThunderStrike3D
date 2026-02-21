using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public int amount = 1;

    void OnTriggerEnter(Collider other)
    {
        // ✅ 用根物体判断玩家
        var root = other.transform.root;
        if (!root.CompareTag("Player")) return;

        var inv = root.GetComponent<PlayerInventory>();
        if (inv != null)
        {
            inv.AddHeart(amount);
            Destroy(gameObject);
        }
    }
}

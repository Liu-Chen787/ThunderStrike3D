using UnityEngine;

public class LightingPickup : MonoBehaviour
{
    public int amount = 1;

    void OnTriggerEnter(Collider other)
{
    Debug.Log($"[Pickup] ENTER item={name} other={other.name} otherTag={other.tag} rootTag={other.transform.root.tag}");

    var root = other.transform.root;

    var inv = root.GetComponent<PlayerInventory>() ?? other.GetComponent<PlayerInventory>() ?? other.GetComponentInParent<PlayerInventory>();
    Debug.Log(inv == null
        ? "[Pickup] PlayerInventory NOT FOUND"
        : "[Pickup] PlayerInventory FOUND");

    if (inv == null) return;

    inv.AddLightning(amount);
    Debug.Log("[Pickup] AddLightning OK, destroy item.");
    Destroy(gameObject);
}

}

using UnityEngine;

public class DropOnDeath : MonoBehaviour
{
    [Header("Drop Prefabs")]
    public GameObject heartPrefab;
    public GameObject lightningPrefab;

    [Header("Drop Settings")]
    [Range(0f, 1f)] public float dropChance = 0.6f;
    [Range(0f, 1f)] public float heartWeight = 0.5f;

    [Header("Z Plane")]
    public float dropZ = 0f;   // ✅ 统一玩家平面

    public void TryDrop(Vector3 pos)
    {
        
        // 概率判定
        float roll = Random.value;
        Debug.Log($"[DropOnDeath] Roll value={roll}");

        if (roll > dropChance)
        {
            Debug.Log("[DropOnDeath] No drop this time (failed chance).");
            return;
        }

        // 决定掉落类型
        GameObject drop = null;
        float typeRoll = Random.value;
        

        if (typeRoll < heartWeight)
        {
            drop = heartPrefab;
           
        }
        else
        {
            drop = lightningPrefab;
            
        }

        if (drop == null)
        {
            
            return;
        }

        // 轻微随机偏移
        pos += new Vector3(
            Random.Range(-0.5f, 0.5f),
            Random.Range(-0.2f, 0.2f),
            0f
        );

        // 🔥 强制投影到玩家平面
        pos.z = dropZ;

        Debug.Log($"[DropOnDeath] Final spawn position = {pos}");

        GameObject go = Instantiate(drop, pos, Quaternion.identity);

        
    }
}

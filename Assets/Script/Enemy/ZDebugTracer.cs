using UnityEngine;

public class ZDebugTracer : MonoBehaviour
{
    void Awake()
    {
        Debug.Log($"[ZTrace] {name} Awake z={transform.position.z} path={GetPath(transform)}");
    }

    void OnEnable()
    {
        Debug.Log($"[ZTrace] {name} OnEnable z={transform.position.z} path={GetPath(transform)}");
    }

    void Start()
    {
        Debug.Log($"[ZTrace] {name} Start z={transform.position.z} path={GetPath(transform)}");
    }

    void LateUpdate()
    {
        // 每隔30帧打印一次，避免刷屏
        if (Time.frameCount % 30 == 0)
            Debug.Log($"[ZTrace] {name} LateUpdate z={transform.position.z}");
    }

    static string GetPath(Transform t)
    {
        string path = t.name;
        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}

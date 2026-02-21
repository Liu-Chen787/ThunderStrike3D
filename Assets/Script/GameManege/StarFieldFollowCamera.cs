using UnityEngine;

public class StarFieldFollowCamera : MonoBehaviour
{
    public Transform cam;
    public float forwardOffset = 10f;

    void Start()
    {
        if (!cam) cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (!cam) return;
        transform.position = cam.position + cam.forward * forwardOffset;
        transform.rotation = cam.rotation;
    }
}


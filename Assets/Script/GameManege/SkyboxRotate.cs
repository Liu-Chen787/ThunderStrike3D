using UnityEngine;

public class SkyboxRotate : MonoBehaviour
{
    public float rotationSpeed = 1.5f; // 每秒旋转角度，1~5都可以试
    float _rot;

    void Update()
    {
        _rot += rotationSpeed * Time.deltaTime;
        RenderSettings.skybox.SetFloat("_Rotation", _rot);
    }
}

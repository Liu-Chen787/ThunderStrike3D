using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Limits")]
    public Vector2 limitX = new Vector2(-10f, 10f);
    public Vector2 limitY = new Vector2(-10f, 10f);

    [Header("Speed")]
    public float baseSpeed = 10f;   // 基础速度
    float _speedMul = 1f;           // 技能倍率（默认1）

    // 被 PlayerHealth 调用
    public void SetSpeedMultiplier(float m)
    {
        _speedMul = Mathf.Max(0.1f, m);
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        float h = 0f, v = 0f;

        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)  h -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) h += 1f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed)    v += 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed)  v -= 1f;

        Vector2 input = new Vector2(h, v);
        if (input.sqrMagnitude > 1f) input.Normalize();

        // ✅ 正确使用倍率
        float finalSpeed = baseSpeed * _speedMul;

        transform.position += new Vector3(input.x, input.y, 0f) * finalSpeed * Time.deltaTime;

        // 限制边界
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, limitX.x, limitX.y);
        p.y = Mathf.Clamp(p.y, limitY.x, limitY.y);
        transform.position = p;
    }
}

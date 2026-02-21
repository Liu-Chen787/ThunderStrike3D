using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [Header("Counts")]
    public int heartCount = 0;
    public int lightningCount = 0;

    [Header("Use Effects")]
    public int healAmount = 20;

    [Header("UI (Bottom Bar)")]
    public TMP_Text heartText;      // 例如显示: x2
    public TMP_Text lightningText;  // 例如显示: x1

    PlayerHealth _playerHealth;

    void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        RefreshUI();
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        // E：使用心形回血
        if (kb.eKey.wasPressedThisFrame)
        {
            UseHeart();
        }

        // Q：使用闪电恢复技能
        if (kb.qKey.wasPressedThisFrame)
        {
            UseLightning();
        }
    }

    public void AddHeart(int amount = 1)
    {
        heartCount += amount;
        if (heartCount < 0) heartCount = 0;
        RefreshUI();
    }

    public void AddLightning(int amount = 1)
    {
        lightningCount += amount;
        if (lightningCount < 0) lightningCount = 0;
        RefreshUI();
    }

    void UseHeart()
    {
        if (heartCount <= 0) return;
        if (_playerHealth == null) return;

        heartCount--;
        _playerHealth.Heal(healAmount);
        RefreshUI();
    }

    void UseLightning()
    {
        if (lightningCount <= 0) return;
        if (_playerHealth == null) return;

        lightningCount--;
        _playerHealth.ResetSkillCooldown();
        RefreshUI();
    }

    void RefreshUI()
    {
        if (heartText) heartText.text = $"x{heartCount}";
        if (lightningText) lightningText.text = $"x{lightningCount}";
    }
}

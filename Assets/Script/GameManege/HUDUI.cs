using UnityEngine;
using TMPro;

public class HUDUI : MonoBehaviour
{
    [Header("TMP References")]
    public TMP_Text hpText;
    public TMP_Text skillCDText;

    [Header("Optional")]
    public PlayerHealth playerHealth;

    float _cd;
    float _boost;

    void Awake()
    {
        if (!playerHealth)
            playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    void OnEnable()
    {
        if (playerHealth == null) return;

        playerHealth.onHpChanged += OnHpChanged;
        playerHealth.onSkillCooldownChanged += OnSkillCdChanged;
        playerHealth.onBoostTimeChanged += OnBoostChanged;

        // 立刻刷新一次
        OnHpChanged(playerHealth.hp, playerHealth.maxHP);
        OnSkillCdChanged(playerHealth.CooldownRemaining);
        OnBoostChanged(playerHealth.BoostRemaining);
    }

    void OnDisable()
    {
        if (playerHealth == null) return;

        playerHealth.onHpChanged -= OnHpChanged;
        playerHealth.onSkillCooldownChanged -= OnSkillCdChanged;
        playerHealth.onBoostTimeChanged -= OnBoostChanged;
    }

    void Update()
    {
        // 双保险：就算事件没触发，也每帧从PlayerHealth读一遍
        if (!playerHealth) return;

        _cd = playerHealth.CooldownRemaining;
        _boost = playerHealth.BoostRemaining;
        RefreshSkillText();
    }

    void OnHpChanged(int hp, int maxHP)
    {
        if (hpText) hpText.text = $"HP: {hp}/{maxHP}";
    }

    void OnSkillCdChanged(float remaining)
    {
        _cd = remaining;
        RefreshSkillText();
    }

    void OnBoostChanged(float remainingBoost)
    {
        _boost = remainingBoost;
        RefreshSkillText();
    }

    void RefreshSkillText()
    {
        if (!skillCDText) return;

        if (_boost > 0.01f)
            skillCDText.text = $"Boost: {_boost:0}s (x1.5)";
        else if (_cd > 0.01f)
            skillCDText.text = $"Skill CD: {_cd:0}s";
        else
            skillCDText.text = "Skill: READY (F)";
    }
}

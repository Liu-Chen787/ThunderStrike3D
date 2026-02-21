using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int hp = 100;

    [Header("Skill: Speed Boost")]
    public float speedMultiplier = 1.5f;   // +50% 速度
    public float boostDuration = 10f;      // 持续10秒
    public float skillCooldown = 60f;      // 冷却60秒

    float _cooldownTimer; // 冷却剩余
    float _boostTimer;    // 加速剩余

    // 让HUD可以随时读取（双保险）
    public float CooldownRemaining => _cooldownTimer;
    public float BoostRemaining => _boostTimer;

    public System.Action<int, int> onHpChanged;            // (hp, maxHP)
    public System.Action<float> onSkillCooldownChanged;    // remaining seconds
    public System.Action<float> onBoostTimeChanged;        // remaining boost seconds

    PlayerMovement _mover;

    void Start()
    {
        hp = Mathf.Clamp(hp, 0, maxHP);
        _cooldownTimer = 0f;
        _boostTimer = 0f;

        _mover = GetComponent<PlayerMovement>();

        onHpChanged?.Invoke(hp, maxHP);
        onSkillCooldownChanged?.Invoke(0f);
        onBoostTimeChanged?.Invoke(0f);
    }

    void Update()
    {
        // 冷却倒计时
        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer < 0f) _cooldownTimer = 0f;
            onSkillCooldownChanged?.Invoke(_cooldownTimer);
        }

        // 加速持续时间
        if (_boostTimer > 0f)
        {
            _boostTimer -= Time.deltaTime;
            if (_boostTimer < 0f) _boostTimer = 0f;
            onBoostTimeChanged?.Invoke(_boostTimer);

            if (_boostTimer <= 0f)
            {
                // 结束加速
                if (_mover != null) _mover.SetSpeedMultiplier(1f);
            }
        }

        // F 释放技能
        var kb = Keyboard.current;
        if (kb != null && kb.fKey.wasPressedThisFrame)
        {
            TrySpeedBoost();
        }
    }

    public void TakeDamage(int dmg)
    {
        if (hp <= 0) return;

        hp -= dmg;
        if (hp < 0) hp = 0;

        onHpChanged?.Invoke(hp, maxHP);

        if (hp <= 0)
        {
            GameManager.Instance?.GameOver(false);
        }
    }

    void TrySpeedBoost()
    {
        // 冷却中，不能放
        if (_cooldownTimer > 0f) return;

        // 开启加速
        _boostTimer = boostDuration;
        onBoostTimeChanged?.Invoke(_boostTimer);

        if (_mover != null) _mover.SetSpeedMultiplier(speedMultiplier);

        // 进入冷却
        _cooldownTimer = skillCooldown;
        onSkillCooldownChanged?.Invoke(_cooldownTimer);
    }
    public void Heal(int amount)
    {
        if (hp <= 0) return;

        hp += amount;
        if (hp > maxHP) hp = maxHP;

        onHpChanged?.Invoke(hp, maxHP);
    }

    public void ResetSkillCooldown()
    {
        _cooldownTimer = 0f;
        onSkillCooldownChanged?.Invoke(_cooldownTimer);
    }

}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int hp = 100;

    [Header("Skill: Speed Boost")]
    public float speedMultiplier = 1.5f;
    public float boostDuration   = 10f;
    public float skillCooldown   = 60f;

    [Header("Hit Flash")]
    public Color hitFlashColor = Color.red;   // Inspector 可改颜色

    float _cooldownTimer;
    float _boostTimer;

    public float CooldownRemaining => _cooldownTimer;
    public float BoostRemaining    => _boostTimer;

    public System.Action<int, int> onHpChanged;
    public System.Action<float>    onSkillCooldownChanged;
    public System.Action<float>    onBoostTimeChanged;

    PlayerMovement _mover;
    DamageFlash    _flash;

    void Start()
    {
        hp = Mathf.Clamp(hp, 0, maxHP);
        _cooldownTimer = 0f;
        _boostTimer    = 0f;

        _mover = GetComponent<PlayerMovement>();

        // 找 DamageFlash（可挂在玩家根节点或子节点）
        _flash = GetComponentInChildren<DamageFlash>();

        onHpChanged?.Invoke(hp, maxHP);
        onSkillCooldownChanged?.Invoke(0f);
        onBoostTimeChanged?.Invoke(0f);
    }

    void Update()
    {
        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer < 0f) _cooldownTimer = 0f;
            onSkillCooldownChanged?.Invoke(_cooldownTimer);
        }

        if (_boostTimer > 0f)
        {
            _boostTimer -= Time.deltaTime;
            if (_boostTimer < 0f) _boostTimer = 0f;
            onBoostTimeChanged?.Invoke(_boostTimer);

            if (_boostTimer <= 0f)
                if (_mover != null) _mover.SetSpeedMultiplier(1f);
        }

        var kb = Keyboard.current;
        if (kb != null && kb.fKey.wasPressedThisFrame)
            TrySpeedBoost();
    }

    public void TakeDamage(int dmg)
    {
        if (hp <= 0) return;

        hp -= dmg;
        if (hp < 0) hp = 0;

        // 受击闪红
        if (_flash != null)
        {
            _flash.flashColor = hitFlashColor;
            _flash.Flash();
        }

        onHpChanged?.Invoke(hp, maxHP);

        if (hp <= 0)
            GameManager.Instance?.GameOver(false);
    }

    void TrySpeedBoost()
    {
        if (_cooldownTimer > 0f) return;

        _boostTimer = boostDuration;
        onBoostTimeChanged?.Invoke(_boostTimer);

        if (_mover != null) _mover.SetSpeedMultiplier(speedMultiplier);

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
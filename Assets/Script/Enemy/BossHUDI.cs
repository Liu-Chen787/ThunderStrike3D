using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHUDUI : MonoBehaviour
{
    public static BossHUDUI Instance { get; private set; }

    [Header("Health bar (at the bottom of the Canvas)")]
    public Slider hpSlider;
    public Image  fillImage;
    public TMP_Text phaseText;

    [Header("Phase colors")]
    public Color colorPhase1 = new Color(0.27f, 0.80f, 0.40f); // 绿
    public Color colorPhase2 = new Color(1.00f, 0.67f, 0.13f); // 橙
    public Color colorPhase3 = new Color(0.93f, 0.27f, 0.27f); // 红

    void Awake()
    {
        Instance = this;
    }

    public void UpdateHP(float ratio, int phase)
    {
        if (hpSlider) hpSlider.value = ratio;

        if (fillImage)
            fillImage.color = phase switch
            {
                2 => colorPhase2,
                3 => colorPhase3,
                _ => colorPhase1
            };

        if (phaseText)
            phaseText.text = $"PHASE {phase}";
    }
}
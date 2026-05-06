using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIFlowController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject startPanel;
    public GameObject endPanel;

    [Header("HUD Roots")]
    public GameObject hudRoot;
    public GameObject itemBarRoot;

    [Header("Gameplay Root")]
    public GameObject gameplayRoot;

    [Header("End Panel Text")]
    public TMP_Text titleText;
    public TMP_Text infoText;

    [Header("Next Level")]
    public GameObject nextLevelButton;
    [SerializeField] private string nextLevelSceneName = "Level2";

    [Header("Boss HUD")]
    public GameObject bossHUD;   // ← 声明在这里，Inspector 里拖入 BossHUD 对象

    bool _gameEnded;

    void Start()
    {
        _gameEnded = false;

        if (startPanel)      startPanel.SetActive(true);
        if (endPanel)        endPanel.SetActive(false);
        if (nextLevelButton) nextLevelButton.SetActive(false);
        if (bossHUD)         bossHUD.SetActive(false);  // 默认隐藏

        SetGameplayActive(false);
        SetHudActive(false);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        if (startPanel) startPanel.SetActive(false);
        SetGameplayActive(true);
        SetHudActive(true);
        if (bossHUD) bossHUD.SetActive(true);  // 游戏开始时显示 Boss 血条
        Time.timeScale = 1f;
    }

    // ── 死亡时调用：隐藏 Next Level ─────────────────
    public void ShowGameOver(string title = "GAME OVER", string info = "")
    {
        if (_gameEnded) return;
        _gameEnded = true;

        if (endPanel) endPanel.SetActive(true);
        if (bossHUD)  bossHUD.SetActive(false);
        SetHudActive(false);
        Time.timeScale = 0f;

        if (titleText) titleText.text = title;
        if (infoText)  infoText.text  = info;

        if (nextLevelButton) nextLevelButton.SetActive(false);
    }

    // ── 胜利时调用：显示 Next Level ─────────────────
    public void ShowVictory(string title = "MISSION COMPLETE", string info = "")
    {
        if (_gameEnded) return;
        _gameEnded = true;

        if (endPanel) endPanel.SetActive(true);
        if (bossHUD)  bossHUD.SetActive(false);
        SetHudActive(false);
        Time.timeScale = 0f;

        if (titleText) titleText.text = title;
        if (infoText)  infoText.text  = info;

        if (nextLevelButton) nextLevelButton.SetActive(true);
    }

    // ── 按钮绑定 ─────────────────────────────────────
    public void OnNextLevelClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextLevelSceneName);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void SetGameplayActive(bool on)
    {
        if (gameplayRoot) gameplayRoot.SetActive(on);
    }

    void SetHudActive(bool on)
    {
        if (hudRoot)     hudRoot.SetActive(on);
        if (itemBarRoot) itemBarRoot.SetActive(on);
    }
}
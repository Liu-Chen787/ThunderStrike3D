using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIFlowController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject startPanel;   // Canvas/StartPanel
    public GameObject endPanel;     // Canvas/EndPanel

    [Header("Optional HUD Roots")]
    public GameObject hudRoot;      // Canvas/HUD
    public GameObject itemBarRoot;  // Canvas/ItemBar

    [Header("Gameplay Root")]
    public GameObject gameplayRoot; // GameplayRoot（玩家/刷怪/敌人/星空等）

    [Header("End Panel Text (Optional)")]
    public TMP_Text titleText;      // 例如：GAME OVER
    public TMP_Text infoText;       // 例如：Score: xxx / hint

    bool _gameStarted;
    bool _gameEnded;

    void Start()
    {
        _gameStarted = false;
        _gameEnded = false;

        // 初始：显示开始界面，隐藏结束界面，暂停玩法
        if (startPanel) startPanel.SetActive(true);
        if (endPanel) endPanel.SetActive(false);

        SetGameplayActive(false);
        SetHudActive(false);

        Time.timeScale = 0f;
    }

    public void StartGame()
{
    Debug.Log("StartGame()");

    if (startPanel) startPanel.SetActive(false);
    if (endPanel) endPanel.SetActive(false);

    if (gameplayRoot) gameplayRoot.SetActive(true);

    if (hudRoot) hudRoot.SetActive(true);
    if (itemBarRoot) itemBarRoot.SetActive(true);

    Time.timeScale = 1f;
}

    // GameManager 在“游戏结束”时调用这个（不区分胜负）
    public void ShowGameOver(string title = "GAME OVER", string info = "")
    {
        if (_gameEnded) return;
        _gameEnded = true;

        if (startPanel) startPanel.SetActive(false);
        if (endPanel) endPanel.SetActive(true);

        // HUD 可选隐藏（更干净）
        SetHudActive(false);

        // 游戏暂停（但UI仍可点击）
        Time.timeScale = 0f;

        if (titleText) titleText.text = title;
        if (infoText) infoText.text = info;
    }

    // Restart按钮绑定这个
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Exit按钮绑定这个
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
        if (hudRoot) hudRoot.SetActive(on);
        if (itemBarRoot) itemBarRoot.SetActive(on);
    }
}
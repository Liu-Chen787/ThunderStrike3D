using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI (HUD)")]
    public TMP_Text scoreText;
    public TMP_Text hpText;
    public TMP_Text skillText;

    [Header("UI Flow")]
    public UIFlowController ui; // 拖场景里的 UIFlowController 进来（不要放进 GameplayRoot）

    [Header("Win Goals")]
    public int targetScore = 200;
    public int normalGoal = 16;
    public int eliteGoal = 2;

    [Header("Elite Spawn Rule (Rule A)")]
    public int eliteEveryNormalKills = 5;
    public EnemySpawnerLimited spawner;

    int _score;
    int _normalKills;
    int _eliteKills;
    bool _ended;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        _score = 0;
        _normalKills = 0;
        _eliteKills = 0;
        _ended = false;

        UpdateHUD();
    }

    // ====== Score ======
    public void AddScore(int amount)
    {
        if (_ended) return;

        _score += amount;
        if (_score < 0) _score = 0;

        UpdateHUD();
        CheckWin();
    }

    // ====== Kill (兼容旧调用：不带参数默认普通怪) ======
    public void AddKill()
    {
        AddKill(false);
    }

    // ====== Kill (新：区分普通/精英) ======
    public void AddKill(bool isElite)
    {
        if (_ended) return;

        if (isElite) _eliteKills++;
        else
        {
            _normalKills++;

            if (eliteEveryNormalKills > 0 && _normalKills % eliteEveryNormalKills == 0)
            {
                if (spawner != null) spawner.TrySpawnElite();
                else Debug.LogWarning("GameManager: spawner is null, cannot spawn elite.");
            }
        }

        UpdateHUD();
        CheckWin();
    }

    void CheckWin()
{
    if (_ended) return;

    bool win =
        _score >= targetScore &&
        _normalKills >= normalGoal &&
        _eliteKills >= eliteGoal;

    if (win)
    {
        _ended = true;
        string info = $"Score: {_score}\nNormal: {_normalKills}  Elite: {_eliteKills}";

        if (ui != null)
            ui.ShowVictory("MISSION COMPLETE", info); // ← 改这里
        else
            Time.timeScale = 0f;
    }
}

// 死亡仍然调 ShowGameOver（不变）
public void GameOver()
{
    if (_ended) return;
    _ended = true;

    string info = $"Score: {_score}\nNormal: {_normalKills}/{normalGoal}  Elite: {_eliteKills}/{eliteGoal}";

    if (ui != null)
        ui.ShowGameOver("GAME OVER", info);
    else
        Time.timeScale = 0f;
}
    // ====== 可选：给HUD用 ======
    public void SetHPText(string s)
    {
        if (hpText) hpText.text = s;
    }

    public void SetSkillText(string s)
    {
        if (skillText) skillText.text = s;
    }

    void UpdateHUD()
    {
        if (scoreText)
            scoreText.text = $"Score: {_score}  N:{_normalKills}/{normalGoal}  E:{_eliteKills}/{eliteGoal}";
    }
    // Boss 专用胜利入口（Level 2 使用）
    public void ShowVictoryFromBoss()
{
    if (_ended) return;
    _ended = true;

    string info = $"Command Carrier destroyed!\nScore: {_score}";
    if (ui != null)
        ui.ShowVictory("MISSION COMPLETE", info);
    else
        Time.timeScale = 0f;
}

    // 如果你其他地方仍旧在调用 GameOver(true/false)，保留一个兼容入口（可删）
    public void GameOver(bool win)
    {
        GameOver();
    }
}
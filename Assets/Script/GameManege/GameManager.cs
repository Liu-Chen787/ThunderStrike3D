using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI (HUD)")]
    public TMP_Text scoreText;   // 左上角/右上角都行
    public TMP_Text hpText;
    public TMP_Text skillText;

    [Header("End Panel")]
    public GameObject endPanel;
    public TMP_Text resultText;
    public TMP_Text hintText;

    [Header("Win Goals")]
    public int targetScore = 200;     // 分数目标
    public int normalGoal = 16;       // 普通敌机击杀目标
    public int eliteGoal = 2;         // 精英击杀目标

    [Header("Elite Spawn Rule (Rule A)")]
    public int eliteEveryNormalKills = 5;  // 每击杀多少普通怪刷1只精英（0=关闭）
    public EnemySpawnerLimited spawner;    // 你的刷怪器（拖引用）

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

        Time.timeScale = 1f;

        if (endPanel) endPanel.SetActive(false);
        UpdateHUD();
    }

    void Update()
    {
        if (!_ended) return;

        var kb = Keyboard.current;
        if (kb != null && kb.rKey.wasPressedThisFrame)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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

            // 规则A：固定节奏刷精英
            if (eliteEveryNormalKills > 0 &&
                _normalKills % eliteEveryNormalKills == 0)
            {
                if (spawner != null)
                {
                    spawner.TrySpawnElite();
                }
                else
                {
                    Debug.LogWarning("GameManager: spawner is null, cannot spawn elite.");
                }
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

        if (win) GameOver(true);
    }

    /// <summary>
    /// win=true 表示胜利；win=false 表示失败
    /// </summary>
    public void GameOver(bool win)
    {
        if (_ended) return;
        _ended = true;

        Time.timeScale = 0f;

        if (endPanel) endPanel.SetActive(true);

        if (resultText)
            resultText.text = win ? "YOU WIN!" : "GAME OVER";

        if (hintText)
        {
            string detail = win
                ? $"Score {_score}/{targetScore} | Normal {_normalKills}/{normalGoal} | Elite {_eliteKills}/{eliteGoal}"
                : "Try again!";

            hintText.text = (win ? GetEncourageText() : "Try again!") +
                            "\n" + detail +
                            "\nPress R to Restart";
        }

        Debug.Log($"GameOver called. win={win} score={_score} normalKills={_normalKills} eliteKills={_eliteKills}");
    }

    // ====== 可选：给HUD用（如果你想从GM直接刷新HP/Skill文本） ======
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
        {
            // 显示更完整：分数+击杀进度
            scoreText.text = $"Score: {_score}  N:{_normalKills}/{normalGoal}  E:{_eliteKills}/{eliteGoal}";
        }
    }

    string GetEncourageText()
    {
        string[] tips =
        {
            "Great job, pilot!",
            "Nice shooting!",
            "Well done!",
            "You cleared the mission!"
        };
        return tips[Random.Range(0, tips.Length)];
    }
}

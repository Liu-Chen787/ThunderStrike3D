using UnityEngine;
using TMPro;

public class EndPanelUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text titleText;
    public TMP_Text detailText;
    public TMP_Text hintText;

    void Awake()
    {
        if (panel != null) panel.SetActive(false);
    }

    public void Show(bool win, int killed, int total)
    {
        if (panel != null) panel.SetActive(true);

        if (titleText != null)
            titleText.text = win ? "VICTORY!" : "GAME OVER";

        if (detailText != null)
            detailText.text = $"Enemies Cleared: {killed}/{total}";

        if (hintText != null)
            hintText.text = "Press R to Restart";
    }
}

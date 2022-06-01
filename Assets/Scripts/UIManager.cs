using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    public GameObject panelStart;
    public GameObject panelStats;
    public TMPro.TMP_Text timer;
    public TMPro.TMP_Text score;
    public Image oxygenLine;
    public GameObject scoreField;
    public TMPro.TMP_Text finishScore;
    public TMPro.TMP_Text finishTime;
    public TMPro.TMP_Text finishTotal;

    private float _maxWidthOxygenLine;
    private float _colorOxygenLine;
    
    void Awake()
    {
        UIManager.Instance = this;
    }

    void Start()
    {
        _maxWidthOxygenLine = oxygenLine.rectTransform.sizeDelta.x;
    }

    public static void Init()
    {
        Instance.panelStart.SetActive(false);
        Instance.timer.SetText("00000.00");
        Instance.score.SetText("0");
        Instance.panelStats.SetActive(true);
    }

    public static void ShowResults(string score, string time, string total)
    {
        Instance.panelStats.SetActive(false);
        
        Instance.finishScore.SetText(score);
        Instance.finishTime.SetText(time);
        Instance.finishTotal.SetText(total);
        Instance.panelStart.SetActive(true);
        Instance.scoreField.SetActive(true);
    }

    public static void SetTimerUI(string time)
    {
        Instance.timer.SetText(time);
    }

    public static void SetCurrentScoreUI(string score)
    {
        Instance.score.SetText(score);
    }

    public static void DrawOxygenLine(float value, float maxValue, bool isCritical)
    {
        float currentWidth = (value * Instance._maxWidthOxygenLine) / maxValue;
        Instance.oxygenLine.rectTransform.sizeDelta = new Vector2(currentWidth, Instance.oxygenLine.rectTransform.sizeDelta.y);

        if (isCritical) {
            Instance.oxygenLine.color = new Color(165/255f, 48/255f, 48/255f, 1f);
        } else {
            Instance.oxygenLine.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    public GameObject panelStart;
    public GameObject timerPanel;
    public TMPro.TMP_Text timer;
    public TMPro.TMP_Text score;
    public GameObject scoreField;
    public TMPro.TMP_Text finishScore;
    public TMPro.TMP_Text finishTime;
    public TMPro.TMP_Text finishTotal;
    
    void Awake()
    {
        UIManager.Instance = this;
    }

    public static void Init()
    {
        Instance.panelStart.SetActive(false);
        Instance.timer.SetText("00000.00");
        Instance.score.SetText("0");
        Instance.timerPanel.SetActive(true);
    }

    public static void ShowResults(string score, string time, string total)
    {
        Instance.timerPanel.SetActive(false);
        
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
}

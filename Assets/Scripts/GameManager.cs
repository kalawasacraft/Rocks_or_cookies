using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private float _myScore = 0f;
    private float _timeAlive = 0f;
    private bool _timerGoing = false;

    private bool _isAppStart = false;

    private string _nicknamePrefs = "Nickname";
    private string _highScorePrefs = "HighScore";

    void Awake()
    {
        GameManager.Instance = this;
    }

    void Start()
    {
        if (_isAppStart) {
            _isAppStart = false;
        } else {
            InitGame();
        }
    }

    private void InitGame()
    {
        UIManager.Init();
        KalawasaController.Init();
        JunksPooling.Init(2f);
        CameraZoom.SetZoom(false);
    }

    public static void FinishGame()
    {
        Instance._timerGoing = false;
        Instance.Invoke("LoadFinishGame", 1f);
    }

    private void LoadFinishGame()
    {
        float scoreTime = _timeAlive / 10f;
        float total = scoreTime + _myScore;

        SetMyHighestScore(Mathf.Max(GetMyHighestScore(), total));

        UIManager.ShowResults(((int) _myScore).ToString(), scoreTime.ToString("0.00"), total.ToString("0.00"));
        CameraZoom.SetZoom(true);
    }

    public static void AddToMyScore(float value)
    {
        Instance._myScore += value;
        UIManager.SetCurrentScoreUI(((int) Instance._myScore).ToString());
    }

    public static float GetMyHighestScore()
    {
        if (PlayerPrefs.HasKey(Instance._highScorePrefs)) {
            return PlayerPrefs.GetFloat(Instance._highScorePrefs);
        }
        return 0f;
    }

    public static void SetMyHighestScore(float value)
    {
        PlayerPrefs.SetFloat(Instance._highScorePrefs, value);
        PlayerPrefs.Save();
    }

    public static string GetMyNickname()
    {
        if (PlayerPrefs.HasKey(Instance._nicknamePrefs)) {
            return PlayerPrefs.GetString(Instance._nicknamePrefs);
        }
        return "--";
    }

    public static void SetMyNickname(string value)
    {
        PlayerPrefs.SetString(Instance._nicknamePrefs, value);
        PlayerPrefs.Save();
    }

    public static void BeginTimer()
    {
        Instance._timerGoing = true;
        Instance.StartCoroutine(Instance.UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {        
        while (_timerGoing) {
            _timeAlive += Time.deltaTime;

            UIManager.SetTimerUI(_timeAlive.ToString("00000.00"));
            yield return null;
        }

        //UIManager.Finish(false, _myScore, 0f);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnAppStart()
    {
        Instance._isAppStart = true;
    }
}

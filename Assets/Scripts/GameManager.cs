using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Leaderboard leaderboard;

    public static GameManager Instance;
    [SerializeField] private float _timeOxygen;
    [SerializeField] private int _timeChangeDifficult;

    private float _myScore = 0f;
    private float _timeAlive = 0f;
    private bool _timerGoing = false;
    private float _currentTimeOxygen;
    private bool _oxygenStatus = true;
    private int _currentLevel = 0;

    private bool _isAppStart = false;

    private string _nicknamePrefs = "Nickname";
    private string _highScorePrefs = "HighScore";
    private string _playerIDPrefs = "PlayerID";

    void Awake()
    {
        GameManager.Instance = this;
    }

    void Start()
    {
        _currentTimeOxygen = _timeOxygen;

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
        int scoreTime = ((int)_timeAlive) / 10;
        int total = scoreTime + ((int)_myScore);

        SetMyHighestScore(Mathf.Max(GetMyHighestScore(), total));
        StartCoroutine(leaderboard.SubmitScoreRoutine(total));

        UIManager.ShowResults(((int) _myScore).ToString(), scoreTime.ToString(), total.ToString());
        CameraZoom.SetZoom(true);
    }

    public static void AddToMyScore(float value)
    {
        if (Instance._timerGoing) {
            Instance._myScore += value;
            UIManager.SetCurrentScoreUI(((int) Instance._myScore).ToString());
        }
    }

    public static int GetMyHighestScore()
    {
        if (PlayerPrefs.HasKey(Instance._highScorePrefs)) {
            return PlayerPrefs.GetInt(Instance._highScorePrefs);
        }
        return 0;
    }

    public static void SetMyHighestScore(int value)
    {
        PlayerPrefs.SetInt(Instance._highScorePrefs, value);
        PlayerPrefs.Save();
    }

    public static string GetMyNickname()
    {
        if (PlayerPrefs.HasKey(Instance._nicknamePrefs)) {
            return PlayerPrefs.GetString(Instance._nicknamePrefs);
        }
        return "";
    }

    public static void SetMyNickname(string value)
    {
        PlayerPrefs.SetString(Instance._nicknamePrefs, value);
        PlayerPrefs.Save();
    }

    public static void SetMyPlayerID(string value)
    {
        PlayerPrefs.SetString(Instance._playerIDPrefs, value);
        PlayerPrefs.Save();
    }

    public static string GetMyPlayerID()
    {
        return PlayerPrefs.GetString(Instance._playerIDPrefs);            
    }

    public static void BeginTimer()
    {
        Instance._timerGoing = true;
        Instance.StartCoroutine(Instance.UpdateTimer());
        Instance.StartCoroutine(Instance.UpdateOxygen());
    }

    public static void SetChargeOxygen(bool value)
    {
        Instance._oxygenStatus = value;
    }

    private IEnumerator UpdateTimer()
    {
        int level = _currentLevel;

        while (_timerGoing) {
            _timeAlive += Time.deltaTime;

            UIManager.SetTimerUI(_timeAlive.ToString("00000.00"));

            level = ((int)_timeAlive) / _timeChangeDifficult;
            if (level != _currentLevel) {
                _currentLevel = level;
                JunksPooling.NextLevel();
            }
            yield return null;
        }
    }

    private IEnumerator UpdateOxygen()
    {
        bool isCritical = false;
        while (_currentTimeOxygen > 0) {
            
            if (_oxygenStatus) {
                _currentTimeOxygen = Mathf.Min(_currentTimeOxygen + 2 * Time.deltaTime, _timeOxygen);
            } else {
                _currentTimeOxygen -= Time.deltaTime;
            }
            
            if (_currentTimeOxygen <= _timeOxygen/4) {
                isCritical = ((int) (_currentTimeOxygen*5) % 2 == 0);
            } else {
                isCritical = false;
            }

            if (!_timerGoing) {
                _currentTimeOxygen = 0f;
            }

            UIManager.DrawOxygenLine(Mathf.Max(0f, _currentTimeOxygen), _timeOxygen, isCritical);
            yield return null;
        }

        KalawasaController.WithoutOxygen();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnAppStart()
    {
        Instance._isAppStart = true;
    }
}

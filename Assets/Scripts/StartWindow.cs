using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartWindow : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _highestScoreText;

    void Start()
    {
        _highestScoreText.SetText(GameManager.GetMyHighestScore().ToString("0.00"));
    }

    void Update()
    {
        if (!KalawasaController.GetIsInit() && Input.GetKeyDown(KeyCode.Return)) {
            LoadRestart();      
        }
    }

    public void Restart()
    {
        //PlayConfirmSound();
        Time.timeScale = 1f;
        Invoke("LoadRestart", 0.2f);
    }

    private void LoadRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private float _myScore = 0f;
    private float _myHighScore = 0f;
    private float _timeAlive = 0f;
    private bool _timerGoing = false;

    void Awake()
    {
        GameManager.Instance = this;
    }

    void Start()
    {
        Invoke("InitGame", 1f);
        //Invoke("FinishGame", 5f);
    }

    private void InitGame()
    {
        KalawasaController.Init();
        JunksPooling.Init(2f);
        CameraZoom.SetZoom(false);
    }

    private void FinishGame()
    {
        CameraZoom.SetZoom(true);
    }
}

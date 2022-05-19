using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunksPooling : MonoBehaviour
{
    public static JunksPooling Instance;

    public GameObject prefab;
    [SerializeField] private int _amount;
    [SerializeField] private float _apertureValue;
    [SerializeField] private Vector2 _rangeSpeedRotation;
    [SerializeField] private Vector2 _rangeMoveForce;
    [SerializeField] private Vector2 _rangeInstantiateGap;
    [SerializeField] private float _valueChangeDifficult;
    [SerializeField] private float _offCameraDistance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializePool();
    }

    public static void Init(float delayTime)
    {
        Instance.Invoke("GetJunkFromPool", delayTime);
    }

    private void InitializePool()
    {
        for (int i = 0; i < _amount; i++) {
            AddJunkPool();
        }
    }

    private void AddJunkPool()
    {
        GameObject junk = Instantiate(prefab, this.transform.position, Quaternion.identity, this.transform);
        junk.SetActive(false);
    }

    private void GetJunkFromPool()
    {
        GameObject junk = null;

        for (int i = 0; i < transform.childCount; i++) {
            if (!transform.GetChild(i).gameObject.activeSelf) {
                junk = transform.GetChild(i).gameObject;
                break;
            }
        }

        if (junk == null) {
            AddJunkPool();
            junk = transform.GetChild(transform.childCount - 1).gameObject;
        }        

        (Vector2, Vector2) newPositionDir = GetPositionAndDirection();

        junk.transform.position = newPositionDir.Item1;
        junk.GetComponent<JunkController>().Init(Random.Range(_rangeSpeedRotation[0], _rangeSpeedRotation[1]),
                                                Random.Range(_rangeMoveForce[0], _rangeMoveForce[1]),
                                                newPositionDir.Item2);

        Invoke("GetJunkFromPool", Random.Range(_rangeInstantiateGap[0], _rangeInstantiateGap[1]));
    }

    private (Vector2, Vector2) GetPositionAndDirection()
    {
        int indexCameraSide = Random.Range(0, 4);
        Vector2 bottomLeft, topRight, direction;

        switch(indexCameraSide) {
            case 0:
                bottomLeft = new Vector2(0, Screen.height + _offCameraDistance);
                topRight = new Vector2(Screen.width, Screen.height + _offCameraDistance + 1);
                direction = new Vector2(Random.Range(-_apertureValue, _apertureValue), -1f);
                break;
            case 1:
                bottomLeft = new Vector2(Screen.width + _offCameraDistance, 0);
                topRight = new Vector2(Screen.width + _offCameraDistance + 1, Screen.height);
                direction = new Vector2(-1f, Random.Range(-_apertureValue, _apertureValue));
                break;
            case 2:
                bottomLeft = new Vector2(0, 0 - _offCameraDistance);
                topRight = new Vector2(Screen.width, 0 - _offCameraDistance + 1);
                direction = new Vector2(Random.Range(-_apertureValue, _apertureValue), 1f);
                break;
            default:
                bottomLeft = new Vector2(0 - _offCameraDistance, 0);
                topRight = new Vector2(0 + _offCameraDistance + 1, Screen.height);
                direction = new Vector2(1f, Random.Range(-_apertureValue, _apertureValue));
                break;
        }

        Vector3 worldMin = Camera.main.ScreenToWorldPoint(bottomLeft);
        Vector3 worldMax = Camera.main.ScreenToWorldPoint(topRight);
        Vector2 position = new Vector2(Random.Range(worldMin.x, worldMax.x), Random.Range(worldMin.y, worldMax.y));

        return (position, direction);
    }

    public static void NextLevel()
    {
        Debug.Log("NextLevel!!");
        Instance._rangeMoveForce[0] = Instance._rangeMoveForce[0] + ((Instance._rangeMoveForce[1] - Instance._rangeMoveForce[0]) * Instance._valueChangeDifficult);
        Instance._rangeInstantiateGap[1] = Instance._rangeInstantiateGap[1] - ((Instance._rangeInstantiateGap[1] - Instance._rangeInstantiateGap[0]) * Instance._valueChangeDifficult);

        Debug.Log(Instance._rangeMoveForce[0]);
        Debug.Log(Instance._rangeInstantiateGap[1]);
    }
}

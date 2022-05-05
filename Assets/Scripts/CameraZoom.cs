using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public static CameraZoom Instance;

    public float _speed;
    public Vector2 _sizeCam;

    private bool _zoomActive = true;
    private CinemachineVirtualCamera _vCam;

    void Awake()
    {
        Instance = this;
        
        _vCam = GetComponent<CinemachineVirtualCamera>();
    }

    void LateUpdate()
    {
        if (_zoomActive) {
            _vCam.m_Lens.OrthographicSize = Mathf.Lerp(_vCam.m_Lens.OrthographicSize, _sizeCam[0], _speed);
        } else {
            _vCam.m_Lens.OrthographicSize = Mathf.Lerp(_vCam.m_Lens.OrthographicSize, _sizeCam[1], _speed);
        }
    }

    public static void SetZoom(bool value)
    {
        Instance._zoomActive = value;

        if (Instance._zoomActive) {
            
            Transform _playerTransform = KalawasaController.GetPlayerTransform();

            Instance.transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y - 1f, Instance.transform.position.z);
            Instance._vCam.Follow = null;
        } else {
            Instance._vCam.Follow = KalawasaController.GetPlayerTransform();
        }
    }
}

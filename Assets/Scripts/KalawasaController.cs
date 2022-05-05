using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalawasaController : MonoBehaviour
{
    public static KalawasaController Instance;

    [SerializeField] private float _stunTime;
    [SerializeField] private float _navigationForce;
    [SerializeField] private float _maxVelocity;

    public ParticleSystem navigatorRight;
    public ParticleSystem navigatorLeft;
    public ParticleSystem navigatorTop;
    public ParticleSystem navigatorDown;

    private Animator _animator;
    private Rigidbody2D _rigidbody;

    private bool _isInit = false;
    private float _horizontalInput;
    private float _verticalInput;

    private string _readyAnimationTriggerName = "Ready";
    private string _happyAnimationTriggerName = "Happy";
    private string _hitAnimationTriggerName = "Hit";
    private string _deathAnimationTriggerName = "Death";

    void Awake()
    {
        Instance = this;

        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        //_audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_isInit) {

            if (Input.GetButtonDown("Fire1")) {
                Debug.Log("Impulse Go!!!");
            }
        }
    }

    void FixedUpdate()
    {
        if (_isInit) {

            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalInput = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(_rigidbody.velocity.x) > _maxVelocity && _horizontalInput * _rigidbody.velocity.x > 0f) {
                _horizontalInput = 0;
            }
            if (Mathf.Abs(_rigidbody.velocity.y) > _maxVelocity && _verticalInput * _rigidbody.velocity.y > 0f) {
                _verticalInput = 0;
            }
            _rigidbody.AddForce(new Vector2(_horizontalInput, _verticalInput) * _navigationForce, ForceMode2D.Impulse);

            Debug.Log(_rigidbody.velocity);
        }
    }

    void LateUpdate()
    {
        if (_horizontalInput > 0) {
            navigatorRight.Play();
        } else if (_horizontalInput < 0) {
            navigatorLeft.Play();
        }

        if (_verticalInput > 0) {
            navigatorTop.Play();
        } else if (_verticalInput < 0) {
            navigatorDown.Play();
        }
    }

    public static void Init()
    {
        Instance._animator.SetTrigger(Instance._readyAnimationTriggerName);
    }

    public static Transform GetPlayerTransform()
    {
        return Instance.transform;
    }

    public void Initiate()
    {
        _isInit = true;
    }
}

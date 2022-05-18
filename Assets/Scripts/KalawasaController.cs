using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalawasaController : MonoBehaviour
{
    public static KalawasaController Instance;

    [SerializeField] private float _navigationForce;
    [SerializeField] private float _maxVelocity;

    public ParticleSystem navigatorRight;
    public ParticleSystem navigatorLeft;
    public ParticleSystem navigatorTop;
    public ParticleSystem navigatorDown;
    public GameObject charge;
    [SerializeField] private List<Material> _chargesParticles;
    [SerializeField] private List<Color> _colorChargesParticles;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _sprite;

    private int _chargeValue = -1;
    private bool _isInit = false;
    private float _horizontalInput;
    private float _verticalInput;

    private string _readyAnimationTriggerName = "Ready";
    private string _happyAnimationTriggerName = "Happy";
    private string _hitAnimationTriggerName = "Hit";
    private string _deathAnimationTriggerName = "Death";
    private string _tagDeathName = "Death";

    void Awake()
    {
        Instance = this;

        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        //_audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_isInit) {

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
        }
    }

    void LateUpdate()
    {
        if (_isInit) {
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

            if (_animator.GetCurrentAnimatorStateInfo(0).IsTag(_tagDeathName)) {
                GameManager.FinishGame();
                _isInit = false;
            }
        }
    }

    public static void Init()
    {
        Instance._animator.SetTrigger(Instance._readyAnimationTriggerName);
    }

    public static bool GetIsInit()
    {
        return Instance._isInit;
    }

    public void HitJunk()
    {
        if (_isInit) {
            _animator.SetTrigger(_hitAnimationTriggerName);
            _sprite.color = new Color(117f/255f, 36f/255f, 56f/255f, 1f);
            Invoke("RestartDeafultColor", 0.25f);
        }
    }

    public void Colonize()
    {
        if (_isInit) {
            _animator.SetTrigger(_happyAnimationTriggerName);
        }
    }

    public static void WithoutOxygen()
    {
        if (Instance._isInit) {
            Instance._animator.SetTrigger(Instance._deathAnimationTriggerName);
        }
    }

    public void RestartDeafultColor()
    {
        _sprite.color = new Color(1f, 1f, 1f, 1f);
    }

    public static Transform GetPlayerTransform()
    {
        return Instance.transform;
    }

    public static int GetChargeValue()
    {
        return Instance._chargeValue;
    }

    public static void SetChargeValue(int value)
    {
        Instance._chargeValue = value;
        
        Material currentMaterial = null;
        ParticleSystem.MinMaxGradient currentColor = null;
        
        if (Instance._chargeValue != -1) {
            currentMaterial = Instance._chargesParticles[Instance._chargeValue];
            currentColor = Instance._colorChargesParticles[Instance._chargeValue];
        }
        
        ParticleSystem.MainModule settings = Instance.charge.GetComponent<ParticleSystem>().main;
        settings.startColor = currentColor;
        Instance.charge.GetComponent<ParticleSystemRenderer>().material = currentMaterial;
    }

    public static void ChargeForce(Vector2 direction, float maxMagneticForce)
    {
        float tDistance = 2f, tForce = 0.001f;
        float currentDistance = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
        Debug.Log(currentDistance);
        
        float newForce = ((tDistance - currentDistance) * maxMagneticForce + currentDistance * tForce) / tDistance;
        
        Instance._rigidbody.AddForce(direction * newForce, ForceMode2D.Impulse);
    }

    public void Initiate()
    {
        _isInit = true;
        GameManager.BeginTimer();
    }
}

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
    public AudioSource _audioHurtBox;
    public AudioClip _clipDeathHurtBox;
    public AudioSource _audioHitBox;
    public AudioClip _clipCharge;
    public AudioClip _clipDischarge;
    public AudioSource _audioCharge;
    public AudioSource _audioRightNav;
    public AudioSource _audioLeftNav;
    public AudioSource _audioTopNav;
    public AudioSource _audioDownNav;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _sprite;
    private AudioSource _audio;

    private int _chargeValue = -1;
    private bool _isInit = false;
    private float _horizontalInput;
    private float _verticalInput;
    private bool _isNavR = false, _isNavL = false, _isNavU = false, _isNavD = false;

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
        _audio = GetComponent<AudioSource>();
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
                if (!navigatorRight.isEmitting) {
                    navigatorRight.Play();
                }
                if (!_isNavR) {
                    _audioRightNav.Play();
                    _isNavR = true;
                }
            } else if (_horizontalInput < 0) {
                if (!navigatorLeft.isEmitting) {
                    navigatorLeft.Play();
                }
                if (!_isNavL) {
                    _audioLeftNav.Play();
                    _isNavL = true;
                }
            } else {
                if (_isNavR) {
                    _audioRightNav.Stop();
                    _isNavR = false;
                } else if (_isNavL) {
                    _audioLeftNav.Stop();
                    _isNavL = false;
                }
            }

            if (_verticalInput > 0) {
                if (!navigatorTop.isEmitting) {
                    navigatorTop.Play();
                }
                if (!_isNavU) {
                    _audioTopNav.Play();
                    _isNavU = true;
                }
            } else if (_verticalInput < 0) {
                if (!navigatorDown.isEmitting) {
                    navigatorDown.Play();
                }
                if (!_isNavD) {
                    _audioDownNav.Play();
                    _isNavD = true;
                }
            } else {
                if (_isNavU) {
                    _audioTopNav.Stop();
                    _isNavU = false;
                } else if (_isNavD) {
                    _audioDownNav.Stop();
                    _isNavD = false;
                }
            }

            if (_animator.GetCurrentAnimatorStateInfo(0).IsTag(_tagDeathName)) {
                GameManager.FinishGame();
                _isInit = false;
            }

        } else {
            _audioRightNav.Stop();
            _audioLeftNav.Stop();
            _audioTopNav.Stop();
            _audioDownNav.Stop();
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
            _audioHurtBox.Play();
            _animator.SetTrigger(_hitAnimationTriggerName);
            _sprite.color = new Color(117f/255f, 36f/255f, 56f/255f, 1f);
            Invoke("RestartDeafultColor", 0.25f);
        }
    }

    public void Colonize()
    {
        if (_isInit) {
            _animator.SetTrigger(_happyAnimationTriggerName);
            _audio.Play();
        }
    }

    public static void WithoutOxygen()
    {
        if (Instance._isInit) {
            Instance._audioHurtBox.Play();
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
        if (Instance._chargeValue != value) {
            if (value != -1) {
                Instance._audioHitBox.clip = Instance._clipCharge;
            } else {
                Instance._audioHitBox.clip = Instance._clipDischarge;
            }
            Instance._audioHitBox.Play();
        }
        
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
        
        float newForce = ((tDistance - currentDistance) * maxMagneticForce + currentDistance * tForce) / tDistance;
        
        Instance._rigidbody.AddForce(direction * newForce, ForceMode2D.Impulse);
    }

    public void Initiate()
    {
        _isInit = true;
        GameManager.BeginTimer();
    }

    public void Death()
    {
        _audioHurtBox.clip = _clipDeathHurtBox;
        _audioHurtBox.Play();
    }

    public static void PlayOxygenCharge(bool value)
    {
        if (value) {
            Instance._audioCharge.Play();
        } else {
            Instance._audioCharge.Stop();
        }
    }
}

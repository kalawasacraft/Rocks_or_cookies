using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSpot : MonoBehaviour
{
    public GameObject charge;
    public GameObject icon;
    public GameObject signIcon;
    public Sprite rockSprite;
    public Sprite plusSprite;
    public Sprite minusSprite;

    [SerializeField] private float _magneticForce;
    [SerializeField] private List<Material> _chargesParticles;
    [SerializeField] private List<Color> _colorChargesParticles;
    [SerializeField] private bool _isOnlySpot = true;
    
    private int _chargeValue = -1;

    private Collider2D _collider;
    private SpriteRenderer _sprite;

    private string _tagPlayerName = "Player";

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();

        Init();        
    }

    public void Init()
    {
        _chargeValue = Random.Range(0, _chargesParticles.Count);

        charge.SetActive(true);
        charge.GetComponent<ParticleSystemRenderer>().material = _chargesParticles[_chargeValue];

        ParticleSystem.MainModule settings = charge.GetComponent<ParticleSystem>().main;
        settings.startColor = _colorChargesParticles[_chargeValue];

        SpriteRenderer spriteIcon = icon.GetComponent<SpriteRenderer>();

        if (!_isOnlySpot) {
            _sprite.color = new Color(_colorChargesParticles[_chargeValue].r,
                                        _colorChargesParticles[_chargeValue].g,
                                        _colorChargesParticles[_chargeValue].b,
                                        _sprite.color.a);
            
            signIcon.SetActive(false);
            spriteIcon.sprite = rockSprite;
            spriteIcon.color = _colorChargesParticles[_chargeValue];
        } else {
            spriteIcon.color = new Color(87/255f, 114/255f, 119/255f, 0.7f);

            SpriteRenderer spriteSignIcon = signIcon.GetComponent<SpriteRenderer>();
            spriteSignIcon.sprite = (_chargeValue == 0 ? minusSprite : plusSprite);
            spriteSignIcon.color = _colorChargesParticles[_chargeValue];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag(_tagPlayerName)) {
            if (_isOnlySpot) {
                KalawasaController.SetChargeValue(_chargeValue);
                
                GameManager.SetChargeOxygen(true);
            } else {
                if (_chargeValue == -1) {
                    KalawasaController.SetChargeValue(_chargeValue);

                    GameManager.SetChargeOxygen(true);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(_tagPlayerName)) {
            if (!_isOnlySpot) {
                if (_chargeValue != -1) {
                    int currentChargeValue = KalawasaController.GetChargeValue();
                    if (_chargeValue == currentChargeValue) {
                        KalawasaController.ChargeForce(CalculateDirection(collision.transform.position, true), _magneticForce);
                    } else if (_chargeValue + currentChargeValue == 1) {
                        KalawasaController.ChargeForce(CalculateDirection(collision.transform.position, false), _magneticForce);
                    }
                    
                    GameManager.SetChargeOxygen(false);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(_tagPlayerName)) {
            if (_isOnlySpot) {
                GameManager.SetChargeOxygen(false);
            } else {
                if (_chargeValue == -1) {
                    GameManager.SetChargeOxygen(false);
                }
            }
        }
    }

    public void ChargeToNeutro()
    {
        _chargeValue = -1;
        _sprite.color = new Color(1f, 1f, 1f, _sprite.color.a);
        icon.GetComponent<SpriteRenderer>().color = new Color(87/255f, 114/255f, 119/255f);

        charge.SetActive(false);
    }

    private Vector2 CalculateDirection(Vector3 position, bool type)
    {
        return (type ? 1 : -1) * (new Vector2(position.x - transform.position.x, position.y - transform.position.y));
    }
}

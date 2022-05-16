using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    public GameObject pointsToPanel;
    public TMPro.TMP_Text textPointsToPanel;
    [SerializeField] private Vector3 _offsetToPanel;
    [SerializeField] private GameObject _spotCharge;
    [SerializeField] private bool _isColonize = false;

    private Animator _animator;
    private Collider2D _collider;
    private SpriteRenderer _sprite;

    private int _points = 4;
    private bool _isFirstCollisionPlayer = false;

    private string _collisionAnimationTriggerName = "Collision";
    private string _explosionAnimationTriggerName = "Explosion";
    private string _tagPlayerName = "Player";
    private string _functionColonizeName = "Colonize";

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (_isColonize) {
            Colonized();
        } else {
            transform.localScale = new Vector3(1.9f, 1.9f, 1f);
        }
        textPointsToPanel.SetText(_points.ToString());
    }

    void Update()
    {
        if (!_isFirstCollisionPlayer) {
            pointsToPanel.transform.position = Camera.main.WorldToScreenPoint(transform.position + _offsetToPanel);
        }
    }

    public void Init(float scaleValue, int pointsValue)
    {
        _points = pointsValue;
        textPointsToPanel.SetText(_points.ToString());
        transform.localScale = new Vector3(scaleValue, scaleValue, 1f);
        // set scale, points, charge
    }

    private void Colonized()
    {
        _isFirstCollisionPlayer = true;
        pointsToPanel.SetActive(false);

        _spotCharge.GetComponent<ChargeSpot>().ChargeToNeutro();
        _sprite.color = new Color(87/255f, 114/255f, 119/255f, 1f);
    }

    public void ShowCollisionJunk()
    {
        if (!_isFirstCollisionPlayer) {
            _points -= 2;
            
            if (_points == 0) {
                _isFirstCollisionPlayer = true;
                pointsToPanel.SetActive(false);
                _animator.SetTrigger(_explosionAnimationTriggerName);
            } else {
                _animator.SetTrigger(_collisionAnimationTriggerName);
                textPointsToPanel.SetText(_points.ToString());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.tag == _tagPlayerName && !_isFirstCollisionPlayer) {
            
            int currentChargeValue = KalawasaController.GetChargeValue();
            if (currentChargeValue != -1) {

                Colonized();

                KalawasaController.SetChargeValue(-1);
                GameManager.AddToMyScore(_points);
                GameManager.SetChargeOxygen(true);

                collision.gameObject.SendMessageUpwards(_functionColonizeName);
            }
        }
    }

    public void ExplosionRock()
    {
        Destroy(gameObject);
    }
}

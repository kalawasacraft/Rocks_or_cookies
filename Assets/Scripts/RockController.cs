using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    [SerializeField] private GameObject _spotCharge;
    [SerializeField] private bool _isColonize = false;

    private Animator _animator;
    private Collider2D _collider;
    private SpriteRenderer _sprite;

    private int _points = 0;
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
        }
    }

    public void Init()
    {
        // set scale, points, charge
    }

    private void Colonized()
    {
        _isFirstCollisionPlayer = true;

        _spotCharge.GetComponent<ChargeSpot>().ChargeToNeutro();
        _sprite.color = new Color(87f/255f, 114f/255f, 119f/255f, 1f);
    }

    public void ShowCollisionJunk()
    {
        _animator.SetTrigger(_collisionAnimationTriggerName);
        // less value's rock
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.tag == _tagPlayerName && !_isFirstCollisionPlayer) {
            
            int currentChargeValue = KalawasaController.GetChargeValue();
            if (currentChargeValue != -1) {

                Colonized();
                // add value's rock to score
                KalawasaController.SetChargeValue(-1);
                GameManager.SetChargeOxygen(true);

                collision.gameObject.SendMessageUpwards(_functionColonizeName);
            }
        }
    }
}

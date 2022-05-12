using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkController : MonoBehaviour
{
    [SerializeField] private List<Sprite> _images;
    [SerializeField] private float _timeLife;

    public ParticleSystem dustParticles;

    private float _speedRotation;
    private float _moveForce;
    private Vector2 _direction;
    private bool _moveOn = false;
    private bool _isFirstCollisionPlayer = false;
    private bool _isFirstCollisionRock = false;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _sprite;
    private Collider2D _collider;

    private string _tagPlayerName = "Player";
    private string _tagRockName = "Rock";
    private string _functionHitName = "HitJunk";
    private string _functionCollisionName = "ShowCollision";

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (_moveOn) {
            transform.Rotate(new Vector3(0f, 0f, 1f) * _speedRotation * Time.deltaTime);
        }
    }

    public void Init(float speedRotation, float moveForce, Vector2 direction)
    {
        _isFirstCollisionPlayer = false;
        _isFirstCollisionRock = false;
        _sprite.color = new Color(1f, 1f, 1f, 1f);
        _sprite.sprite = _images[Random.Range(0, _images.Count)];
        _speedRotation = speedRotation;
        _moveForce = moveForce;
        _direction = direction;
        _moveOn = true;
        gameObject.SetActive(true);

        _collider.enabled = true;
        _rigidbody.AddForce(_direction * _moveForce, ForceMode2D.Impulse);

        StartCoroutine(ScreenTime());
    }

    private void Stop()
    {
        StopAllCoroutines();
        dustParticles.Play();

        _sprite.color = new Color(1f, 1f, 1f, 0f);
        _rigidbody.velocity = Vector2.zero;
        _collider.enabled = false;
        _moveOn = false;
        
        Invoke("Inactive", 0.4f);
    }

    private void Inactive()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.tag == _tagPlayerName && !_isFirstCollisionPlayer) {

            _isFirstCollisionPlayer = true;
            Stop();
            
            collision.gameObject.SendMessageUpwards(_functionHitName);
        } else if (collision.gameObject.tag == _tagRockName && !_isFirstCollisionRock) {

            _isFirstCollisionRock = true;
            Stop();

            collision.gameObject.SendMessageUpwards(_functionCollisionName);
        }
    }

    private IEnumerator ScreenTime()
    {
        float currentTimeLife = 0f;

        while (currentTimeLife < _timeLife) {
            currentTimeLife += Time.deltaTime;

            yield return null;
        }

        while (_sprite.isVisible) {
            yield return null;
        }
        
        Stop();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkController : MonoBehaviour
{
    [SerializeField] private List<Sprite> _images;
    [SerializeField] private float _timeLife;

    private float _speedRotation;
    private float _moveForce;
    private Vector2 _direction;
    private bool _moveOn = false;
    private bool _isFirstCollisionPlayer = false;
    private bool _isFirstCollisionRock = false;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _sprite;

    private string _tagPlayerName = "Player";
    private string _tagRockName = "Rock";
    private string _functionHitName = "HitJunk";

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (_moveOn) {
            transform.Rotate(new Vector3(0f, 0f, 1f) * _speedRotation * Time.deltaTime);
        }
    }

    public void Init(float speedRotation, float moveForce, Vector2 direction)
    {
        //_rigidbody.velocity = Vector2.zero; //change velocity to zero when collision with player or rocks
        _isFirstCollisionPlayer = false;
        _isFirstCollisionRock = false;
        _sprite.sprite = _images[Random.Range(0, _images.Count)];
        _speedRotation = speedRotation;
        _moveForce = moveForce;
        _direction = direction;
        _moveOn = true;
        gameObject.SetActive(true);

        _rigidbody.AddForce(_direction * _moveForce, ForceMode2D.Impulse);

        StartCoroutine(ScreenTime());
    }

    private void Stop()
    {
        StopAllCoroutines();

        _rigidbody.velocity = Vector2.zero;
        _moveOn = false;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.tag == _tagPlayerName && !_isFirstCollisionPlayer) {

            _isFirstCollisionPlayer = true;
            Stop();
            
            collision.gameObject.SendMessageUpwards(_functionHitName);
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

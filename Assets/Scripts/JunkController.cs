using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkController : MonoBehaviour
{
    [SerializeField] private List<Sprite> _images;

    private float _speedRotation;
    private float _moveForce;
    private Vector2 _direction;
    private bool _moveOn = false;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _sprite;

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
        _sprite.sprite = _images[Random.Range(0, _images.Count)];
        _speedRotation = speedRotation;
        _moveForce = moveForce;
        _direction = direction;
        _moveOn = true;
        gameObject.SetActive(true);

        _rigidbody.AddForce(_direction * _moveForce, ForceMode2D.Impulse);
    }
}

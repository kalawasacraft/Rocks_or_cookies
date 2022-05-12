using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    private Animator _animator;
    private Collider2D _collider;

    private string _collisionAnimationTriggerName = "Collision";
    private string _explosionAnimationTriggerName = "Explosion";

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
    }

    public void ShowCollision()
    {
        _animator.SetTrigger(_collisionAnimationTriggerName);
    }
}

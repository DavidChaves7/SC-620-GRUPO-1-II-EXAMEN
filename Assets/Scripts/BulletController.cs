using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    float speed;

    Rigidbody2D _rigidbody;
    float _damage;
    bool _isPercentage;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _rigidbody.velocity = _rigidbody.velocity.normalized * speed;
    }

    public void SetDirection(Vector2 direction)
    {
        _rigidbody.velocity = direction * speed;
    }

    public void SetDamage(float damage, bool isPercentage)
    {
        _damage = damage;
        _isPercentage = isPercentage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DamageController damageController = collision.collider.GetComponent<DamageController>();
        if (damageController != null)
        {
            damageController.TakeDamage(_damage, _isPercentage);
        }
        Destroy(gameObject);
    }

}

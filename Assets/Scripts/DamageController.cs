using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageController : MonoBehaviour
{
    private int ANIMATION_HURT;
    private int ANIMATION_DEAD;

    [SerializeField]
    float maxHealth;
    [SerializeField]
    bool isPlayer;

    Rigidbody2D _rb;
    Animator _animator;

    private float _currentHealth;
   
    private void Awake()
    {
        _currentHealth = maxHealth;
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        ANIMATION_HURT = Animator.StringToHash("hurt");
        ANIMATION_DEAD = Animator.StringToHash("dead");
    }

    public void TakeDamage(float value, bool isPercentage = false)
    {
        float damage = Mathf.Abs(value);
        if (isPercentage)
        {
            damage = damage * 100 / maxHealth;
        }
        _animator.SetTrigger(ANIMATION_HURT);
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
                StartCoroutine(DestroyCoroutine()); 
        }
    }

    public int getHealth()
    {
        if (maxHealth > 0)
            return (int)maxHealth;
        else return 0;
    }

    private IEnumerator DestroyCoroutine()
    {
        _animator.SetTrigger(ANIMATION_DEAD);
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = 0;

        if (isPlayer)
        {
            SoldierController2D soldierController = GetComponent<SoldierController2D>();
            soldierController.enabled = false;

            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;

            yield return new WaitForSeconds(3.0F);
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            levelManager.NextLevel();
        }
        else
        {
            yield return new WaitForSeconds(1.0F);
            Destroy(gameObject);
        }
    }
}


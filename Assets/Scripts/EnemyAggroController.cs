using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroController : MonoBehaviour
{
    [Header("Aggro Mechanic")]
    [SerializeField]
    Transform player;

    [SerializeField]
    float agroRange;

    [SerializeField]
    float maxRangeUntilRetreat;

    [SerializeField]
    float speed;

    [SerializeField]
    Transform restingPoint;

    [SerializeField]
    float secondsUntilRetreat;

    [Header("Attack Mechanic")]
    [SerializeField]
    Transform attackPoint;  // Punto desde el cual se realiza el ataque

    [SerializeField]
    Vector2 attackSize;  // Tamaño del área de ataque

    [SerializeField]
    float attackCooldown;  // Tiempo entre ataques

    [SerializeField]
    int meleeDamage = 33;  // Daño del ataque melee

    private float lastAttackTime; // Para manejar el cooldown de ataque

    Rigidbody2D _rb;
    Animator _animator;
    float _restingTime;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _restingTime = secondsUntilRetreat;
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (player == null)
        {
            StopChasePlayer();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float distanceToRestingPoint = Vector2.Distance(transform.position, restingPoint.position);

        HandleWalkingAnimation();

        if (distanceToPlayer < agroRange)
        {
            ChasePlayer();
            _restingTime = secondsUntilRetreat;

            if (distanceToPlayer <= attackSize.x && Time.time >= lastAttackTime + attackCooldown)
            {
                MeleeAttack();  // Intentar realizar el ataque si el jugador está en rango y ha pasado el cooldown
            }
        }
        else if (distanceToPlayer > (agroRange * 1.3f) || distanceToRestingPoint > maxRangeUntilRetreat)
        {
            StopChasePlayer();
        }
    }

    private void HandleWalkingAnimation()
    {
        if (_rb.velocity.magnitude > 0.1f)
        {
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }
    }

    private void ChasePlayer()
    {
        HandleWalkFacingFromTo(transform, player);
    }

    private void HandleWalkFacingFromTo(Transform pos1, Transform pos2)
    {
        if (pos1.position.x < pos2.position.x)
        {
            _rb.velocity = new Vector2(speed, 0.0f);
            pos1.localScale = new Vector2(1.0f, 1.0f);
        }
        else
        {
            _rb.velocity = new Vector2(-speed, 0.0f);
            pos1.localScale = new Vector2(-1.0f, 1.0f);
        }
    }

    private void StopChasePlayer()
    {
        _rb.velocity = Vector2.zero;
        if (_restingTime <= 0.0f)
        {
            float distanceToRestingPoint = Vector2.Distance(transform.position, restingPoint.position);
            if (distanceToRestingPoint > 0.01f)
            {
                HandleWalkFacingFromTo(_rb.transform, restingPoint);
            }
        }
        else
        {
            _restingTime -= Time.deltaTime;
        }
    }

    private void MeleeAttack()
    {
        _animator.SetTrigger("attack");

        Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0f);

        foreach (Collider2D hitPlayer in hitPlayers)
        {
            DamageController damageController = hitPlayer.GetComponent<DamageController>();
            if (damageController != null)
            {
                // Aplicar daño al jugador
                damageController.TakeDamage(meleeDamage);
                Debug.Log("El zombie golpeó al jugador.");
            }
        }

        // Actualiza el tiempo del último ataque
        lastAttackTime = Time.time;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackSize);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

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

    [Header("Melee attack")]
    [SerializeField]
    float meleeRange;

    private Rigidbody2D _rb;
    private Animator _animator;
    private float _restingTime;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _restingTime = secondsUntilRetreat;
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        

    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            StopChasePlayer();
            return;
        }


        HandleWalkingAnimation();


        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float distanceToRestingPoint = Vector2.Distance(transform.position, restingPoint.position);

        if (distanceToPlayer <= meleeRange)
            _animator.SetTrigger("attack");


        if (distanceToPlayer < agroRange)
        {
            ChasePlayer();
            _restingTime = secondsUntilRetreat;
        }
        else if (distanceToRestingPoint > maxRangeUntilRetreat)
            StopChasePlayer();
        else if (distanceToPlayer > (agroRange * 1.3f))
            StopChasePlayer();
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
        else if (pos1.position.x > pos2.position.x)
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
            if (distanceToRestingPoint > 0.2f)
            {
                HandleWalkFacingFromTo(_rb.transform, restingPoint);
            }
        }
        else
            _restingTime -= Time.deltaTime;
    }

    private void HandleWalkingAnimation()
    {
        if (_rb.velocity.magnitude > 0.1f)
            _animator.SetBool("isWalking",true);
        else
            _animator.SetBool("isWalking", false);
    }
}

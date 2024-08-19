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
        //primero checkear la distancia al jugador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float distanceToRestingPoint = Vector2.Distance(transform.position, restingPoint.position);

        HandleWalkingAnimation();
       
        if (distanceToPlayer < agroRange)
        {
            //persigue al jugador
            ChasePlayer();
            _restingTime = secondsUntilRetreat;
        }
        else if (distanceToPlayer > (agroRange * 1.3F) || distanceToRestingPoint > maxRangeUntilRetreat)
        {
            //parar de perseguir

            StopChasePlayer();
        }
    }

    private void HandleWalkingAnimation()
    {
        if (_rb.velocity.magnitude > 0.1F)
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
        HandleWalkFacingFromTo(transform,player);
    }

    private void HandleWalkFacingFromTo(Transform pos1, Transform pos2)
    {
        if (pos1.position.x < pos2.position.x)
        {
            //se gira y se mueve a la derecha
            _rb.velocity = new Vector2(speed, 0.0F);
            pos1.localScale = new Vector2(1.0F, 1.0F);
        }
        else
        {
            //se gira y se mueve a la izquierda
            _rb.velocity = new Vector2(-speed, 0.0F);
            pos1.localScale = new Vector2(-1.0F, 1.0F);
        }
    }

    private void StopChasePlayer()
    {
        _rb.velocity = Vector2.zero;
        if (_restingTime <= 0.0F)
        {
            float distanceToRestingPoint = Vector2.Distance(transform.position, restingPoint.position);
            if(distanceToRestingPoint > 0.01F)
                HandleWalkFacingFromTo(_rb.transform, restingPoint);

        }
        else
            _restingTime -= Time.deltaTime;


    }


}

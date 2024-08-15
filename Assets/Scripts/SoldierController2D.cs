using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController2D : MonoBehaviour
{
    private int ANIMATION_SPEED;
    private int ANIMATION_FORCE;
    private int ANIMATION_RUN;
    private int ANIMATION_WALK;
    private int ANIMATION_SHOOT;
    private int ANIMATION_ATTACK;
    private int ANIMATION_DEAD;
 

    [Header("Movement")]
    [SerializeField]
    float walkSpeed;
    [SerializeField]
    float runSpeed;
    [SerializeField]
    float jumpForce;

    Rigidbody2D _rigidbody;
    Animator _animator;

    float _inputX;
    float _velocityY;

    bool _inputRun;
    bool _isFacingLeft;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        ANIMATION_ATTACK = Animator.StringToHash("attack");
        ANIMATION_SPEED = Animator.StringToHash("speed");
        ANIMATION_SHOOT = Animator.StringToHash("shoot");
        ANIMATION_DEAD = Animator.StringToHash("dead");
        ANIMATION_RUN = Animator.StringToHash("run");
        ANIMATION_WALK = Animator.StringToHash("walk");

    }
    void Start()
    {
        
    }

    void Update()
    {
        HandleInputMove();
        HandleInputRun();
    }
    private void FixedUpdate()
    {
        HandleMove();
        HandleRotate();
    }

    private void HandleInputMove() 
    {
        _inputX = Input.GetAxisRaw("Horizontal");
    }
    private void HandleInputRun()
    {
        _inputRun = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }
    private void HandleMove()
    {
        float speed = _inputRun ? runSpeed : walkSpeed;

        if (_inputX == 0.0F)
        {
            speed = 0.0F;
        }

        _animator.SetFloat(ANIMATION_SPEED, speed);
        if (_inputRun)
        {
            _animator.SetBool(ANIMATION_RUN, true);
        }
        else
        {
            _animator.SetBool(ANIMATION_RUN, false);
            _animator.SetBool(ANIMATION_WALK, true);
        }
        Vector2 velocity = new Vector2(_inputX, 0.0F) * speed * Time.fixedDeltaTime;
        velocity.y = _velocityY;

        _rigidbody.velocity = velocity;
    }

    private void HandleRotate()
    {
        if (_inputX == 0.0F)
        {
            return;
        }
        bool facingLeft = _inputX < 0.0F;
        if (_isFacingLeft != facingLeft)
        {
            _isFacingLeft = facingLeft;
            transform.Rotate(0.0F, 180.0F, 0.0F);
        }
    }

}

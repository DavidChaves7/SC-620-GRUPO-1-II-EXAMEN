using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SoldierController2D : MonoBehaviour
{
    private int ANIMATION_SPEED;
    private int ANIMATION_FORCE;
    private int ANIMATION_FALL;
    private int ANIMATION_RUN;
    private int ANIMATION_WALK;
    private int ANIMATION_SHOOT;
    private int ANIMATION_ATTACK;
    private int ANIMATION_DEAD;
 
    //////////////////////
    [Header("Movement")]
    [SerializeField]
    float walkSpeed;

    [SerializeField]
    float runSpeed;

    [SerializeField]
    float jumpForce;

    [SerializeField]
    float gravityMultiplier;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    Vector2 groundCheckSize;

    [SerializeField]
    LayerMask groundMask;

    [SerializeField]
    bool isFacingLeft;
    /////////////////////////
    [Header("Attack")]
    [SerializeField]
    Transform attackPoint;

    [SerializeField]
    Vector2 attackSize;

    [SerializeField]
    Transform shotPoint;

    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    float bulletLifeTime;

    [SerializeField]
    float fireTimeout;

    [SerializeField]
    LayerMask attackMask;



    Rigidbody2D _rigidbody;
    Animator _animator;

    [SerializeField, Tooltip("Range interval for attacks")]
    float attackRange;
    [SerializeField, Tooltip("Amount of attacks per Range")]
    int attackRate;


    float _fireTimer;

    float _inputX;
    float _velocityY;
    float _gravityY;
    float _attackTime;

    bool _inputRun;
    bool _isGrounded;
    bool _isJumpPressed;
    bool _isJumping;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        _gravityY = Physics2D.gravity.y;

        ANIMATION_ATTACK = Animator.StringToHash("attack");
        ANIMATION_SPEED = Animator.StringToHash("speed");
        ANIMATION_SHOOT = Animator.StringToHash("shoot");
        ANIMATION_DEAD = Animator.StringToHash("dead");
        ANIMATION_RUN = Animator.StringToHash("run");
        ANIMATION_WALK = Animator.StringToHash("walk");
        ANIMATION_FORCE = Animator.StringToHash("force");
        ANIMATION_FALL = Animator.StringToHash("fall");
    }
    void Start()
    {
        HandleGrounded();
    }

    void Update()
    {
        HandleGravity();
        HandleInputMove();
        HandleInputRun();
        HandleInputAttack();
        HandleInputShoot(); 
    }
    private void FixedUpdate()
    {
        HandleJump();
        HandleRotate();
        HandleMove();
        
        
    }
    private void HandleGravity()
    {
        print("velocity: " + _velocityY + "   //// IsGrounded: " + _isGrounded + "//// isJumping: " + _isJumping);
        if (_isGrounded)
        {
            if (_velocityY < -1.0F)
            {
                _velocityY = -1.0F;
            }
            _isJumpPressed = Input.GetButton("Jump");   
        }
    }
    private void HandleInputMove() 
    {
        _inputX = Input.GetAxisRaw("Horizontal");
    }
    private void HandleInputRun()
    {
        _inputRun = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }
    private void HandleInputAttack ()
    {
        _attackTime -= Time.deltaTime;
        if (_attackTime < 0.0F)
        {
            _attackTime = 0.0F;
        }
        if (_attackTime == 0.0F)
        {
            if (Input.GetButtonUp("Fire2"))
            {
                Attack();
            }
        }
    }  
    private void HandleInputShoot() 
    {
        _fireTimer -= Time.deltaTime;
        if (_fireTimer < 0.0F)
        {
            _fireTimer = 0.0F;
        }
        if (_fireTimer == 0.0F)
        {
            if (Input.GetButtonUp("Fire1"))
            {

                Shoot();

                if (_fireTimer > 0.0F)
                {
                    return;
                }
            }
        }
    }
    private void HandleGrounded()
    {
        _isGrounded = IsGrounded();
        if (!_isGrounded)
        {
            StartCoroutine(WaitForGroundedCoroutine());
        }
    }
    private void HandleJump()
    {
        if (_isJumpPressed)
        {
            _isJumpPressed = false;
            _isGrounded = false;
            _isJumping = true;

            _velocityY = jumpForce;
            _animator.SetTrigger(ANIMATION_FORCE);
            StartCoroutine(WaitForGroundedCoroutine());

        }
        else if (!_isGrounded)
        {
            _velocityY += _gravityY * gravityMultiplier * Time.fixedDeltaTime;
            if (!_isJumping)
            {
                _animator.SetTrigger(ANIMATION_FALL);
            }
        }
        else if (_isGrounded)
        {
            if (_velocityY >= 0.0F)
            {
                _velocityY = -1.0F;
            }
            else
            {
                HandleGrounded();
            }

            _isJumping = false;
        }
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
    private void Attack() 
    { 
       _animator.SetTrigger (ANIMATION_ATTACK);
    }
        public void Attack(float damage, bool isPercentage)
        {
            Collider2D[] colliders =
               Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0F, attackMask);
            foreach (Collider2D collider in colliders)
            {
                DamageController controller = collider.GetComponent<DamageController>();
                if (controller == null)
                {
                    continue;
                }
                controller.TakeDamage(damage, isPercentage);
            }
        }
    private void Shoot() 
    {
        _animator.SetTrigger(ANIMATION_SHOOT);
    }
    public void Shoot(float damage, bool isPercentage)
    {
        GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation);
        BulletController bulletController = bullet.GetComponent<BulletController>();

        if (bulletController != null)
        {
            bulletController.SetDamage(damage, isPercentage);
            bulletController.SetDirection(isFacingLeft ? Vector2.left : Vector2.right);
        }
        Destroy(bullet, bulletLifeTime); 
    }
    private void HandleRotate()
    {
        if (_inputX == 0.0F)
        {
            return;
        }
        bool facingLeft = _inputX < 0.0F;
        if (isFacingLeft != facingLeft)
        {
            isFacingLeft = facingLeft;
            transform.Rotate(0.0F, 180.0F, 0.0F);
        }
    }
    private bool IsGrounded()
    {
        Collider2D collider2D = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0.0F, groundMask);
        return collider2D != null;
    }
    private IEnumerator WaitForGroundedCoroutine()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(() => IsGrounded());
        _isGrounded = true;
    }

}

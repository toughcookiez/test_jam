using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;

    [SerializeField] private float _jumpForce = 4f;

    [Tooltip("all things that the player can stand on should be on this layer")]
    [SerializeField] private LayerMask _groundLayer;


    private Animator _animator;

    private SpriteRenderer _spriteRenderer;

    private Rigidbody2D _rb;

    private float InputX;

    Controls playerControls;

    [Header("GroundCheck Settings")]

    [SerializeField] private float _castDistance;

    [SerializeField] private Vector2 _boxSize;

    private void Awake()
    {
        playerControls = new Controls();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            Debug.LogWarning("Player controls is null.");
            return;
        }
        playerControls.Enable();
    }
    private void OnDisable()
    {
        if (playerControls == null)
        {
            Debug.LogWarning("Player controls is null.");
            return;
        }
        playerControls.Disable();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _animator = GetComponent<Animator>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        playerControls.PlayerActions.Jump.performed += OnJump;
    }

    private void OnDestroy()
    {
        playerControls.PlayerActions.Jump.performed -= OnJump;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded)
        {
            _animator.SetBool("Grounded", false);
            _animator.SetTrigger("Jump");
            
            _rb.linearVelocityY = _jumpForce;
        }
    }

    private void Update()
    {
        Move();
        _animator.SetBool("Grounded", IsGrounded);
    }

    public bool IsGrounded
    {
        get
        {
            if (Physics2D.BoxCast(transform.position, _boxSize, 0, -transform.up, _castDistance, _groundLayer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * _castDistance, _boxSize);
    }


    private void Move()
    {
        InputX = playerControls.PlayerActions.Move.ReadValue<float>();
        if (_spriteRenderer != null)
        {
            if (InputX > 0)
            {
                _animator.SetBool("IsRunning", true);
                _spriteRenderer.flipX = false;
            }
            else if (InputX < 0)
            {
                _animator.SetBool("IsRunning", true);
                _spriteRenderer.flipX = true;
            }
            else if (InputX == 0)
            {
                _animator.SetBool("IsRunning", false);
            }
        }

        if (_rb != null)
        {
            _rb.linearVelocityX = InputX * _speed;
        }
        else
        {
            Debug.LogError("Rigidbody2D is null");
        }
    }
}

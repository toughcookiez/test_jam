using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;

    [SerializeField] private float _jumpForce = 4f;

    [Tooltip("all things that the player can stand on should be on this layer")]
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private float _timeBeforeDeath = 3;

    private Collider2D _collider;

    private bool _enabled = false;


    private Animator _animator;

    private SpriteRenderer _spriteRenderer;

    private Rigidbody2D _rb;

    [Header("Shadow settings")]

    [SerializeField] private GameObject ShadowObject;

    public SpriteRenderer _shadowSpriteRenderer;

    public float _shadowTranstionTime;

    [Header("Flashlight settings")]
    [SerializeField] private int _flashlightCharges = 1;
    [SerializeField] private float _flashlightDuration = 1f;
    [SerializeField] private float _flashlightShadowAmount = 0;



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
        _collider = GetComponent<Collider2D>();

        _rb = GetComponent<Rigidbody2D>();     

        _animator = GetComponent<Animator>();

        _shadowSpriteRenderer = ShadowObject.GetComponent<SpriteRenderer>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        playerControls.PlayerActions.Jump.performed += OnJump;

        playerControls.PlayerActions.LightUp.performed += OnLightUp;
    }

    private void OnLightUp(InputAction.CallbackContext obj)
    {
        if (_flashlightCharges > 0 && _enabled)
        {
            _flashlightCharges--;
            StartCoroutine(LightUp());
        }
    }

    public void Die()
    {
        StartCoroutine(ExecuteDeath());
    }

    private IEnumerator ExecuteDeath()
    {
        _collider.enabled = false;
        _rb.gravityScale = 0;
        //death animation
        yield return new WaitForSeconds(_timeBeforeDeath);
        LevelManager.Instance.RestartLevel();
    }

    public IEnumerator ApplyShadows()
    {
        float elpasedTime = 0;
        while (elpasedTime < _shadowTranstionTime && _shadowTranstionTime > 0)
        {
            _shadowSpriteRenderer.material.SetFloat("_DarknessStrength", Mathf.Lerp(0, 64, elpasedTime / _shadowTranstionTime));
            elpasedTime += Time.deltaTime;
            yield return null;
        }

        _shadowSpriteRenderer.material.SetFloat("_DarknessStrength", Mathf.Lerp(0, 64, 1));
        Enabled = true;
    }

    public IEnumerator DisapplyShadows()
    {
        Enabled = false;
        float elpasedTime = 0;
        while (elpasedTime < _shadowTranstionTime && _shadowTranstionTime > 0)
        {
            _shadowSpriteRenderer.material.SetFloat("_DarknessStrength", Mathf.Lerp(64, 0, elpasedTime / _shadowTranstionTime));
            elpasedTime += Time.deltaTime;
            yield return null;
        }

        _shadowSpriteRenderer.material.SetFloat("_DarknessStrength", Mathf.Lerp(64, 0, 1));
        
    }

    private IEnumerator LightUp()
    {
        _shadowSpriteRenderer.material.SetFloat("_DarknessStrength", 0);

        float elpasedTime = 0;
        while (elpasedTime < _flashlightDuration && _flashlightDuration > 0)
        {
            elpasedTime += Time.deltaTime;
            yield return null;
        }

        elpasedTime = 0;
        float dimDuration = _flashlightDuration / 2;
        while (elpasedTime < dimDuration && _flashlightDuration > 0)
        {
            _shadowSpriteRenderer.material.SetFloat("_DarknessStrength", Mathf.Lerp(0, 64, elpasedTime / dimDuration));
            elpasedTime += Time.deltaTime;
            yield return null;
        }

        _shadowSpriteRenderer.material.SetFloat("_DarknessStrength", Mathf.Lerp(0, 64, 1));
    }

    private void OnDestroy()
    {
        playerControls.PlayerActions.LightUp.performed -= OnLightUp;

        playerControls.PlayerActions.Jump.performed -= OnJump;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded && _enabled)
        {
            _animator.SetBool("Grounded", false);
            _animator.SetTrigger("Jump");
            
            _rb.linearVelocityY = _jumpForce;
        }
    }

    private void Update()
    {
        _animator.SetBool("Grounded", IsGrounded);
        _animator.SetBool("Enabled", _enabled);
        if (!_enabled)
        {
            return;
        }
        Move();
        
        
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

    public bool Enabled { 
        get { 
            return _enabled; 
        } 
        set
        {
            _enabled = value;
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

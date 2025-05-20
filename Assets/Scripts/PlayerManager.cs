using System;
using System.Collections;
using Characters;
using Managers;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Animation")]
    [SerializeField] private bool useAnimation = false;

    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _sr;

    private float _horizontalInput;
    private bool _isGrounded;
    private bool _jumpPressed;

    [SerializeField] private Character characterData;
    private SwordManager _swordManager;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        
        if (useAnimation) _animator = GetComponent<Animator>();

        gameObject.tag = characterData.tag;
        GetComponent<HealthSystem>().Setup(characterData);


        _swordManager = transform.Find("Hand").Find("Sword").GetComponent<SwordManager>();
    }

    private void Start()
    {
        //GameObject.Find("Managers").GetComponent<GameManager>().PlayStartMoment();
    }


    private bool _canMove = false;
    public void SetCanMove(bool value) => _canMove = value;
    
    private int _talkingNpcStep = 0;
    public void SetTalkingNpcStep(int value) => _talkingNpcStep = value;
    private NPCManager _talkableNpc;
    private void Update()
    {
        if (!_canMove) return;
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)) && _isGrounded) _jumpPressed = true;
        
        if (useAnimation && _animator != null) UpdateAnimations();

        if (Input.GetMouseButtonDown(0) && !_swordManager.IsAttacking)
        {
            StartCoroutine(PlayerAttack());
        }

        if (Input.GetKeyDown(KeyCode.E) && _talkableNpc != null)
        {
            GameObject.Find("Game0Manager").GetComponent<Game0Manager>().TalkTo(_talkableNpc, _talkingNpcStep);
            //GameObject.Find("Game0Manager").GetComponent<Game0Manager>().TalkToEctor(0);
        }
    }

    private IEnumerator PlayerAttack()
    {
        _animator.SetBool("NormalAttack", true);
        yield return new WaitForSeconds(0.35f);
        _swordManager.SetAttacking(5f);
        yield return new WaitForSeconds(0.4f);
        _swordManager.StopAttacking();
        _animator.SetBool("NormalAttack", false);
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        Move();
        Jump();
        ApplyJumpPhysics();
    }

    private void Move()
    {
        _rb.linearVelocity = new Vector2(_horizontalInput * moveSpeed, _rb.linearVelocity.y);
        if (_horizontalInput != 0) transform.rotation = _horizontalInput < 0 ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity;
    }

    private void Jump()
    {
        if (_jumpPressed)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            _jumpPressed = false;
        }
    }

    private void ApplyJumpPhysics()
    {
        if (_rb.linearVelocity.y < 0)
        {
            _rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (_rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void UpdateAnimations()
    {
        if (_animator != null)
        {
            _animator.SetFloat("Speed", Mathf.Abs(_horizontalInput));
            //_animator.SetBool("IsGrounded", _isGrounded);
            _animator.SetFloat("VerticalVelocity", _rb.linearVelocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC") 
            && other.TryGetComponent(out NPCInteractions npcInteractions) 
            && other.TryGetComponent(out NPCManager npcManager))
        {
            npcInteractions.ShowToInteract();
            _talkableNpc = npcManager;
            return;
        }
        if (other.TryGetComponent(out SkillTrigger skillTrigger)) skillTrigger.PlayerEnteredTrigger(gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC") && other.TryGetComponent(out NPCInteractions npcInteractions))
        {
            npcInteractions.HideToInteract();
            _talkableNpc = null;
            return;
        }
        if (other.TryGetComponent(out SkillTrigger skillTrigger)) skillTrigger.PlayerLeftTrigger();
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
using System;
using System.Collections;
using Characters;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip slashClip;
    
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
    [SerializeField] private float attackCooldown = 2f;
    private SwordManager _swordManager;
    private float _attackCooldown = 0f;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        
        if (useAnimation) _animator = GetComponent<Animator>();

        gameObject.tag = characterData.tag;
        GetComponent<HealthSystem>().Setup(characterData);


        _swordManager = transform.Find("Hand").Find("Sword").GetComponent<SwordManager>();
    }

    private GameManager _gameManager;
    private void Start()
    {
        try
        {
            _gameManager = GameObject.Find("Managers").GetComponent<GameManager>();
        }
        catch (Exception e)
        {
            return;
        }
    }


    private bool _canMove = false;
    public void SetCanMove(bool value) => _canMove = value;
    
    private int _talkingNpcStep = 0;
    public void SetTalkingNpcStep(int value) => _talkingNpcStep = value;
    private NPCManager _talkableNpc;
    private float _talkingDelay = 0f;
    private House2Scene _house2Scene;
    private Florest1Scene _florest1Scene;
    private Florest2Scene _florest2Scene;
    private GlobalStuff _globalStuff;
    private bool _sprint = false;
    private void Update()
    {
        _attackCooldown -= Time.deltaTime;
        _talkingDelay -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_globalStuff == null)
            {
                GameObject obj = GameObject.Find("SceneManager");
                if (obj != null) _globalStuff = obj.GetComponent<GlobalStuff>();
                if (_globalStuff == null) return;
            }
            _globalStuff.PressedEsc();
        }

        if (!_canMove) return;
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _sprint = Input.GetKey(KeyCode.LeftShift);
        
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)) && _isGrounded) _jumpPressed = true;
        
        if (useAnimation && _animator != null) UpdateAnimations();
        
        if (Input.GetKeyDown(KeyCode.B) && !_swordManager.IsAttacking && _attackCooldown <= 0f)
        {
            StartCoroutine(PlayerAttack());
        }

        if (Input.GetKeyDown(KeyCode.E) && _talkableNpc != null && _talkingDelay <= 0f)
        {
            _talkingDelay = 0.1f; // Delay so it doesn't spam GameObject finds every frame if player spams
            //GameObject.Find("Game0Manager").GetComponent<Game0Manager>().TalkTo(_talkableNpc, _talkingNpcStep);
            //GameObject.Find("Game0Manager").GetComponent<Game0Manager>().TalkToEctor(0);
            if (SceneManager.GetActiveScene().name.Equals("House_2"))
            {
                if (GameObject.Find("SceneManager").TryGetComponent(out House2Scene house2Scene))
                {
                    _house2Scene = house2Scene;
                    if (!house2Scene.AlreadyTalked) house2Scene.TalkingToEctor(_talkableNpc);
                }
            }

            if (SceneManager.GetActiveScene().name.Equals("Florest_1"))
            {
                if (GameObject.Find("SceneManager").TryGetComponent(out Florest1Scene florest1Scene))
                {
                    _florest1Scene = florest1Scene;
                    if (!florest1Scene.AlreadyTalked) florest1Scene.TalkingToEctor(_talkableNpc);
                }
            }

            if (SceneManager.GetActiveScene().name.Equals("Florest_2"))
            {
                if (GameObject.Find("SceneManager").TryGetComponent(out Florest2Scene florest2Scene))
                {
                    _florest2Scene = florest2Scene;
                    if (!florest2Scene.AlreadyTalked) florest2Scene.TalkingToEctor(_talkableNpc);
                }
            }
        }
    }

    private Coroutine _tookDamageCoro;
    public void TookDamage()
    {
        if (_tookDamageCoro != null) StopCoroutine(_tookDamageCoro);
        _tookDamageCoro = StartCoroutine(TookDamageAnimation());
    }

    private IEnumerator TookDamageAnimation()
    {
        Color c = _sr.color;
        c.a = 0.2f;
        _sr.color = c;
        yield return new WaitForSeconds(0.1f);
        Color cc = Color.white * 10f;
        cc.a = 1f;
        _sr.color = cc;
        yield return new WaitForSeconds(0.15f);
        _sr.color = Color.white;
    }

    public void PlaySlashSound()
    {
        if (audioSource != null)
        {
            float pitch = Random.Range(0.7f, 1.2f);
            audioSource.pitch = pitch;
            float volume = _gameManager != null ? _gameManager.Volume : 1f;
            audioSource.PlayOneShot(slashClip, volume);
        }
    }

    private bool _isAttacking = false;
    private IEnumerator PlayerAttack()
    {
        _isAttacking = true;
        _attackCooldown = attackCooldown;
        if (canvasManager != null) canvasManager.StartCooldownNormalAttack(_attackCooldown);
        _animator.SetBool("NormalAttack", true);
        yield return new WaitForSeconds(0.35f);
        _swordManager.SetAttacking(3f);
        _animator.SetBool("NormalAttack", false);
        yield return new WaitForSeconds(0.4f);
        _swordManager.StopAttacking();
        _isAttacking = false;
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
        float xMovement = _horizontalInput * moveSpeed;
        if (_sprint) xMovement *= 3;
        if (_isAttacking) xMovement /= 5f;
        if (!_canMove) xMovement = 0f;
        _rb.linearVelocity = new Vector2(xMovement, _rb.linearVelocity.y);
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
        if (other.gameObject.layer == 7) return;
        if (other.CompareTag("Trigger") && other.gameObject.layer == 9)
        {
            if (other.TryGetComponent(out WalkingBehindEffect walkingBehindEffect))
            {
                walkingBehindEffect.PlayerIsBehind();
            }
            if (other.TryGetComponent(out TriggerCallNPC triggerCallNpc))
            {
                Transform npcTrans = triggerCallNpc.GetNpcTransform();
                if (npcTrans.TryGetComponent(out NPCInteractions npcTransInteractions)
                    && npcTrans.TryGetComponent(out NPCManager npcTransManager))
                {
                    if (SceneManager.GetActiveScene().name.Equals("House_2") && _house2Scene != null)
                    {
                        if (_house2Scene.AlreadyTalked) return;
                    }
                    if (SceneManager.GetActiveScene().name.Equals("Florest_1") && _florest1Scene != null)
                    {
                        if (_florest1Scene.AlreadyTalked) return;
                    }
                    if (SceneManager.GetActiveScene().name.Equals("Florest_2") && _florest2Scene != null)
                    {
                        if (_florest2Scene.AlreadyTalked) return;
                    }
                    npcTransInteractions.ShowToInteract();
                    _talkableNpc = npcTransManager;
                    return;
                }
            }
        }
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
        if (other.CompareTag("Trigger") && other.gameObject.layer == 9)
        {
            if (other.TryGetComponent(out WalkingBehindEffect walkingBehindEffect))
            {
                walkingBehindEffect.PlayerIsNotBehind();
            }
            if (other.TryGetComponent(out TriggerCallNPC triggerCallNpc))
            {
                Transform npcTrans = triggerCallNpc.GetNpcTransform();
                if (npcTrans.TryGetComponent(out NPCInteractions npcTransInteractions))
                {
                    npcTransInteractions.HideToInteract();
                    _talkableNpc = null;
                    return;
                }
            }
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
using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters
{
    public class EnemyManager : MonoBehaviour
    {

        [Header("Movement Settings")]
        [SerializeField] private bool followPlayer = true;
        [SerializeField] private bool lookAtPlayer = false;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float detectionRange = 128f;
        [SerializeField] private bool canSeePlayer = false;
        [SerializeField] private LayerMask obstacleLayer;
        
        [Header("Attack Settings")]
        [SerializeField] private bool canAttack = true;
        [SerializeField] private float attackRange = 32f;
        [SerializeField] private float attackCooldown = 2f;
        [SerializeField] private GameObject AttackPrefab;
        [SerializeField] private float AttackSpeed = 2f;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip slashClip;
        
        [SerializeField] private Transform _playerTransform;
        private Rigidbody2D _rb;
        private bool isMoving = false;
        private Animator _animator;
        private SpriteRenderer _sr;
        private GameManager _gameManager;
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            //_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _sr = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            try
            {
                _gameManager = GameObject.Find("Managers").GetComponent<GameManager>();
            }
            catch (Exception e)
            {
                return;
            }
        }

        private void Update()
        {
            if (_playerTransform == null || !followPlayer) return;
            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
            if (distanceToPlayer <= detectionRange)
            {
                canSeePlayer = !Physics2D.Raycast(transform.position, 
                    (_playerTransform.position - transform.position).normalized, 
                    distanceToPlayer, 
                    obstacleLayer);
                if (canSeePlayer && distanceToPlayer <= attackRange)
                {
                    CheckAttack();
                }
            }
            else
            {
                canSeePlayer = false;
            }
            
            if (!canAttack && Time.time >= _lastAttackTime + attackCooldown)
            {
                canAttack = true;
            }
        }
        
        private void FixedUpdate()
        {
            if (!followPlayer)
            {
                if (lookAtPlayer) LookTowardsPlayer();
                return;
            }
            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
            if (canSeePlayer && _playerTransform != null && _attackAnimeCoro == null && distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                isMoving = false;
                _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            }
        }

        private void LookTowardsPlayer()
        {
            float direction = _playerTransform.position.x > transform.position.x ? 1f : -1f;
            transform.rotation = Quaternion.Euler(0f, direction > 0f ? 0f : 180f, 0f);
        }
        
        private void MoveTowardsPlayer()
        {
            isMoving = true;
            float direction = _playerTransform.position.x > transform.position.x ? 1f : -1f;
            _rb.linearVelocity = new Vector2(direction * moveSpeed, _rb.linearVelocity.y);
        
            transform.rotation = Quaternion.Euler(0f, direction > 0f ? 0f : 180f, 0f);
        }
        
        private float _lastAttackTime = -9999f;
        private void CheckAttack()
        {
            if (canAttack)
            {
                PerformAttack();
                _lastAttackTime = Time.time;
                canAttack = false;
                Cast();
            }
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
        
        private Coroutine _attackAnimeCoro;
        private void EnableAttackAnimation()
        {
            if (_attackAnimeCoro != null) StopCoroutine(_attackAnimeCoro);
            _attackAnimeCoro = StartCoroutine(AttackAnimation());
        }

        private IEnumerator AttackAnimation()
        {
            yield return new WaitForSeconds(1.8f);
            if (_animator) _animator.SetBool("Attack", true);
            yield return new WaitForSeconds(0.1f);
            if (_animator) _animator.SetBool("Attack", false);
            _attackAnimeCoro = null;
        }

        private List<GameObject> _spawnedSkills;
        private bool Cast()
        {
            if (AttackPrefab == null) return false;
            EnableAttackAnimation();
            if (_spawnedSkills == null) _spawnedSkills = new List<GameObject>();
            _spawnedSkills.Add(Instantiate(AttackPrefab, _playerTransform.transform.position, Quaternion.identity));
            return true;
        }
        
        private void PerformAttack()
        {
            _rb.linearVelocity = Vector2.zero;
            isMoving = false;
        }
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}
using System;
using UnityEngine;

namespace Characters
{
    public class EnemyManager : MonoBehaviour
    {

        [Header("Movement Settings")]
        [SerializeField] private bool followPlayer = true;
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
        
        [SerializeField] private Transform _playerTransform;
        private Rigidbody2D _rb;
        private bool isMoving = false;
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            //_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            //spriteRenderer = GetComponent<SpriteRenderer>();
            //animator = GetComponent<Animator>();
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
            if (canSeePlayer && _playerTransform != null)
            {
                MoveTowardsPlayer();
            }
            else
            {
                isMoving = false;
                _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            }
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
        
        private bool Cast()
        {
            if (AttackPrefab == null) return false;
            Instantiate(AttackPrefab, transform.position - new Vector3(32f, 0f, 0f), Quaternion.identity);
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
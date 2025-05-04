using System;
using UnityEngine;

namespace Characters
{
    public class LancelotManager : MonoBehaviour
    {

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float detectionRange = 128f;
        [SerializeField] private bool canSeePlayer = false;
        [SerializeField] private LayerMask obstacleLayer;
    
        [Header("Components")]
        private Transform playerTransform;
        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        
        private bool isFacingRight = true;
        private bool isMoving = false;
        
        [SerializeField] private GameObject AttackPrefab;
        [SerializeField] private float AttackSpeed = 2f;
        
        private bool Cast()
        {
            Instantiate(AttackPrefab, transform.position + new Vector3(32f, 0f, 0f), Quaternion.identity);
            return false;
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            //spriteRenderer = GetComponent<SpriteRenderer>();
            //animator = GetComponent<Animator>();
        }
    
        private void Update()
        {
            if (playerTransform == null) return;
        
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        
            if (distanceToPlayer <= detectionRange)
            {
                canSeePlayer = !Physics2D.Raycast(transform.position, 
                    (playerTransform.position - transform.position).normalized, 
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
        
            if (animator != null)
            {
                animator.SetBool("isMoving", isMoving);
            }
            
            if (!canAttack && Time.time >= lastAttackTime + attackCooldown)
            {
                canAttack = true;
            }
        }


        private float attackRange = 32f;
        private float attackCooldown = 2f;
        private float lastAttackTime = -9999f;
        private bool canAttack = true;
        private void CheckAttack()
        {
            if (canAttack)
            {
                PerformAttack();
                lastAttackTime = Time.time;
                canAttack = false;
            
                // Call your existing attack function here
                Cast();
            }
        }
        
        private void PerformAttack()
        {
            rb.linearVelocity = Vector2.zero;
            isMoving = false;
        }
        
        private void FixedUpdate()
        {
            if (canSeePlayer && playerTransform != null)
            {
                MoveTowardsPlayer();
            }
            else
            {
                isMoving = false;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
    
        private void MoveTowardsPlayer()
        {
            isMoving = true;
        
            float direction = playerTransform.position.x > transform.position.x ? 1f : -1f;
        
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        
            if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
            {
                FlipSprite();
            }
        }
    
        private void FlipSprite()
        {
            isFacingRight = !isFacingRight;
            transform.rotation = Quaternion.Euler(0f, isFacingRight ? 0f : 180f, 0f);
        }
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}
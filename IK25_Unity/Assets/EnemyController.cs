using UnityEngine;
using UnityEngine.AI; // For NavMeshAgent

public class EnemyController : MonoBehaviour
{
    public float health = 100f;
    public float damage = 10f;
    public float attackRange = 1.5f;
    public float damageRange = 2f;
    public float detectionRange = 10f;
    public float attackCooldown = 2f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private float nextAttackTime = 0f;
    private bool isDead = false;

    void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Get components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        // If there's no NavMeshAgent, add one
        if (agent == null)
        {
            agent = gameObject.AddComponent<NavMeshAgent>();
            agent.stoppingDistance = attackRange;
        }
    }

    void Update()
    {
        if (isDead) return;

        // Get distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If player is within detection range
        if (distanceToPlayer <= detectionRange)
        {
            // Set destination to player position
            agent.SetDestination(player.position);
            
            // Set animation parameter for movement
            if (animator != null)
            {
                animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
            }

            if (Time.time <= nextAttackTime) {
                agent.SetDestination(transform.position);
            }
            
            // Check if in attack range and cooldown is over
            if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
            {
                AttackPlayer();
            }
        }
        else
        {
            // Not in detection range, stop moving
            agent.SetDestination(transform.position);
            
            // Set animation parameter for idle
            if (animator != null)
            {
                animator.SetFloat("Speed", 0);
            }
        }
    }
    
    void AttackPlayer()
    {
        // Set next attack time
        nextAttackTime = Time.time + attackCooldown;
        
        // Play attack animation
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    void DealDamage() {
        // Check if player is still in range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= damageRange)
        {
            // Damage player
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
    
    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;
        
        health -= damageAmount;
        
        // Play hit animation
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
        
        // Check if dead
        if (health <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        // Set dead flag
        isDead = true;
        
        // Play death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        
        // Disable NavMeshAgent
        agent.enabled = false;
        
        // Disable collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        
        // Destroy after delay
        Destroy(gameObject, 10f);
    }
}
using UnityEngine;
public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private Animator animator;
    
    private bool isAttacking = false;
    private float attackCooldown = 0f;
    public float attackDuration = 0.5f;
    public float attackCooldownTime = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Get input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        
        // Create movement direction
        moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
        
        // Set animation parameter with a small variation to prevent freezing
        float speedValue = moveDirection.magnitude;
        if (speedValue > 0.01f)
        {
            // Add tiny variation to prevent the animation system from optimizing out updates
            speedValue += Mathf.Sin(Time.time * 10f) * 0.001f;
        }
        animator.SetFloat("Speed", speedValue);
        
        // Handle attack input
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking && attackCooldown <= 0f)
        {
            StartAttack();
        }
        
        // Update attack state
        if (isAttacking)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= attackCooldownTime - attackDuration)
            {
                EndAttack();
            }
        }
        else if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        attackCooldown = attackCooldownTime;
        animator.SetTrigger("Attack");
        
        // Optionally reduce movement speed during attack
        // moveSpeed *= 0.5f;
    }
    
    private void EndAttack()
    {
        isAttacking = false;
        
        // Restore movement speed if you reduced it
        // moveSpeed *= 2f;
    }

    private void FixedUpdate()
    {
        // Apply movement
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        
        // Apply rotation - only if we're actually moving
        if (moveDirection != Vector3.zero)
        {
            // Calculate the target rotation based on move direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            
            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}


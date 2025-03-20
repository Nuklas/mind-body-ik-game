using UnityEngine;
using UnityEngine.UI; // For UI elements

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthBarSlider; // Reference to the Slider
    private Animator animator; // Declare the Animator variable



    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateHealthBar();
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hit");
        
        // Clamp health to avoid negative values
        currentHealth = Mathf.Max(currentHealth, 0);
        
        UpdateHealthBar();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth / maxHealth;
        }
    }
    
    void Die()
    {
        // Game over logic
        Debug.Log("Player died!");
        
        // Optional: Disable player controller
        CharacterMovement movement = GetComponent<CharacterMovement>();
        if (movement != null)
        {
            movement.enabled = false;
        }
        
        
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }
}
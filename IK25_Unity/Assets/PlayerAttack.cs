using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2.5f;
    public float attackDamage = 20f;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    void Start()
    {
        // If no layers are selected, default to Enemy layer
        if (enemyLayers.value == 0)
        {
            enemyLayers = LayerMask.GetMask("Enemy");
        }
    }

    public void DealDamage()
    {
        Debug.Log("DealDamage called");
        
        // Detect enemies in range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        
        Debug.Log("Found " + hitEnemies.Length + " potential targets");
        
        // Damage enemies
        foreach (Collider enemy in hitEnemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            
            if (enemyController != null)
            {
                enemyController.TakeDamage(attackDamage);
                Debug.Log("Hit " + enemy.name);
            }
        }
    }

    // Visualize the attack range in the editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
            
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
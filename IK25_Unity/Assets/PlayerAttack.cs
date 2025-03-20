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

        if (hitEnemies.Length == 0)
        {
            return; // No enemies found, exit function
        }

        // Find the closest enemy
        Collider closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider enemy in hitEnemies)
        {
            float distanceToEnemy = Vector3.Distance(attackPoint.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        // Damage the closest enemy
        if (closestEnemy != null)
        {
            EnemyController enemyController = closestEnemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(attackDamage);
                Debug.Log("Hit closest enemy: " + closestEnemy.name);
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
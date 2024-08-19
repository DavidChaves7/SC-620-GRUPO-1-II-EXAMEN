using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMeleeListener : MonoBehaviour
{
    [SerializeField]
    Transform attackPoint;  // Punto desde el cual se realiza el ataque

    [SerializeField]
    float attackRange = 0.5f;  // Rango del ataque melee

    [SerializeField]
    int meleeDamage = 33;  // Daño del ataque melee

    [SerializeField]
    LayerMask playerLayer;  // La capa a la que pertenece el jugador

    
    public void MeleeAttack()
    {
        
        Collider2D[] objectsOverlapped = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        
        foreach (Collider2D collider in objectsOverlapped)
        {
            if (collider.CompareTag("Player"))
            {
                DamageController damageController = collider.GetComponent<DamageController>();
                if (damageController != null)
                {
                    damageController.TakeDamage(meleeDamage);
                    Debug.Log("El zombie golpeó al jugador.");
                }
            }

           
        }
    }

    
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

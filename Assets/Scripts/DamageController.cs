using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField]
    float maxHealth;

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }
    public void TakeDamage(float value, bool isPercentage = false)
    {
        float damage = Mathf.Abs(value);
        if (isPercentage)
        {
            damage = damage * 100 / maxHealth;
        }

        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int getHealth()
    {
        if (maxHealth > 0)
            return (int) maxHealth;
        else return 0;
    }
}

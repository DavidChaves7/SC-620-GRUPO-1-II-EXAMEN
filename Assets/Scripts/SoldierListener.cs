using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierListener : MonoBehaviour
{
    [SerializeField]
    float meleeDamage;

    [SerializeField]
    bool meleeIsPercentage;

    [SerializeField]
    float rangedDamage;

    [SerializeField]
    bool rangedIsPercentage;

    private SoldierController2D _soldier2D;

    public void Awake()
    {
        _soldier2D = GetComponentInParent<SoldierController2D>();
    }

    public void onAttack()
    {
        _soldier2D.Attack(meleeDamage, meleeIsPercentage);
    }

    public void onShoot()
    {
        _soldier2D.Shoot(rangedDamage, rangedIsPercentage);
    }
}
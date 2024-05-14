using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEnemy : Enemy
{
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private int damage;
    [SerializeField] private GameObject boomEffect;
    
    public override void Update()
    {
        if (CheckIfCanAttack())
        {
            BoomAttack();
        }
    }

    void BoomAttack()
    {
        Collider2D[] detectedObject = Physics2D.OverlapCircleAll(transform.position, attackRadius, whatIsPlayer);
        foreach (Collider2D item in detectedObject)
        {
            item?.GetComponent<Player>()?.Damage(damage);
        }

        Instantiate(boomEffect, transform.position, Quaternion.identity);
        
        Death();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}

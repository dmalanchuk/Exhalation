using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    float timer;
    [SerializeField] private float timeBtwAttack;
    [SerializeField] private Transform shotPos;
    private Transform player;


    [SerializeField] private GameObject bullet;
    
    public override void Start()
    {
        base.Start();       
        timer = timeBtwAttack;
        player = Player.instance.transform;
    }
    
    public override void Update()
    {
        base.Update();
        timer += Time.deltaTime;

         if(CheckIfCanAttack() && player)
         {
            if(timer >= timeBtwAttack)
            {
                timer = 0;
                Shoot();

            }
         }
    }

    void Shoot()
    {
        
        Vector2 direction = player.position - shotPos.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        shotPos.rotation = rotation;

        Instantiate(bullet, shotPos.position, shotPos.rotation);
    }
    
}

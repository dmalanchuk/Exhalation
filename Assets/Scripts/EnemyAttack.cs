using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class EnemyAttack : Enemy
{

    float timer;
    [SerializeField] float timeBtwAttack, attackSpeed;
    [SerializeField] int damage;

    public override void Start()
    {
        base.Start();
        timer = timeBtwAttack;
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
                StartCoroutine(nameof(Attack));

            }
        }
    }

    IEnumerator Attack()
    {
        
        Player.instance.Damage(damage);

        UnityEngine.Vector2 origPos = transform.position;
        UnityEngine.Vector2 plPos = Player.instance.transform.position;

        float percent = 0f;

        while(percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;

            transform.position = UnityEngine.Vector2.Lerp(origPos, plPos, interpolation);
            yield return null;
        }
    }
}

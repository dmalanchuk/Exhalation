using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer spR;

    [SerializeField] int health;
    [SerializeField] float StopDistance, distanceToRunOut, speed;
    protected Player player;
    bool isDeath = false;

    bool canAttack = false;

    [SerializeField] private GameObject hitEffect;
    private Vector3 addRandPosToGo;

    [SerializeField] private ParticleSystem footParticle;
    [SerializeField] private int minCoinsAdd, maxCoinsAdd;
 
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spR = GetComponent<SpriteRenderer>();

        StartCoroutine(nameof(SetRandomPos));
        
        player = Player.instance;
        
        EnemyOrderInLayerManager.instance.Add(spR);
    }

    private void OnDestroy()
    {
        EnemyOrderInLayerManager.instance.Dell(spR);
    }

    public virtual void Update()
    {
        if(isDeath || !player) return;
        if(player) Scale(player.transform.position);
    }
    void FixedUpdate()
    {
        if(isDeath) return;

        if(player && Vector2.Distance(transform.position, player.transform.position) > StopDistance) 
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position + addRandPosToGo, speed * Time.fixedDeltaTime);
            anim.SetBool("Run", true);
            
            var emission = footParticle.emission;
            emission.rateOverTime = 10;
            footParticle.Pause();
            footParticle.Play();

            canAttack = false;
        }
        else if(player && Vector2.Distance(transform.position, player.transform.position) < distanceToRunOut)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position + addRandPosToGo, -speed * Time.fixedDeltaTime);
            anim.SetBool("Run", true);
            
            var emission = footParticle.emission;
            emission.rateOverTime = 10;
            footParticle.Pause();
            footParticle.Play();

            canAttack = false;
        }
        else
        {
            anim.SetBool("Run", false);
            
            var emission = footParticle.emission;
            emission.rateOverTime = 0;
            
            canAttack = true;
        }

    }

    IEnumerator SetRandomPos()
    {
        addRandPosToGo = new Vector3(Random.Range(-StopDistance + 0.1f, StopDistance - 0.1f), Random.Range(-StopDistance + 0.1f, StopDistance - 0.1f));

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(nameof(SetRandomPos));
    }

    void Scale(Vector3 pos)
    {
        if(pos.x >= transform.position.x) transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void Damage(int damage)
    {
        if(isDeath) return;
        health -= damage;

        Instantiate(hitEffect, transform.position, Quaternion.identity);

        if(health <= 0) Death();
    }

    protected void Death()
    {
        isDeath = true;
        
        Player.instance.AddMoney(Random.Range(minCoinsAdd, maxCoinsAdd));
        if(PlayerPrefs.GetInt("Position 1") == 1) Player.instance.AddHealth(1);
        
        anim.SetTrigger("death");
    }

    public  void DestroyObj()
    {
        // while(spR.color.a > 0)
        // {
        //     float p = spR.color.a;
        //     spR.color -= new Color(255f, 255f ,255f, p - 0.1f);
        //     yield return new WaitForSeconds(0.1f);
            
        // }
        Destroy(gameObject);
    }

    public virtual bool CheckIfCanAttack()
    {
        return canAttack && !isDeath;
    }
}

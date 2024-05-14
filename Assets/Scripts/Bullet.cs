using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float deathTime;


    [SerializeField] int damage;
    
    [System.Serializable]
    public enum Type
    {
        Player,
        Enemy
    }

    [SerializeField] private Type type;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Death), deathTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            Death();
        }

        if(collision.gameObject.tag == "Enemy" && type == Type.Player)
        {
            int damagee = PlayerPrefs.GetInt("Position0") == 1 ? damage += 2 : damage;
            collision.gameObject.GetComponent<Enemy>().Damage(damagee);
            Death();
        }
        
        if(collision.gameObject.tag == "Player" && type == Type.Enemy)
        {
            collision.gameObject.GetComponent<Player>().Damage(damage);
            Death();
        }
        
    }
    void Death(){
        Destroy(gameObject);
    }

}

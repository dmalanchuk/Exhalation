using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] int health;
    private int maxHealth;

    private bool canBeDamage = true;
    
    Rigidbody2D rb;
    Vector2 moveVelocity;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform[] shootSuperPos;

    [SerializeField] float timeBtwShoot = 2;
    float shootTimer;

    [SerializeField] float timeBtwSuperShoot = 2;
    float shootSuperTimer;

    Animator anim;
    SpriteRenderer spR;

    [SerializeField] TextMeshProUGUI text;

    public static Player instance;

    [SerializeField] private ParticleSystem footParticle;
    [SerializeField] private GameObject hitEffect;

    [SerializeField] private Sprite[] spritesMuzzleFlash;
    [SerializeField] private SpriteRenderer muzzleFlashSpR;

    [SerializeField] private  float dashForce, timeBtwDash, dashTime;
    private float dashTimer;
    private bool isDashing = false;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider dashSlider;

    [SerializeField] private GameObject deathPanel;

    private Vector2 moveInput;
    
    private void Awake()
    {
        instance = this;

    }

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spR = GetComponent<SpriteRenderer>();

        shootTimer = timeBtwShoot;
        dashTimer = timeBtwDash;
        maxHealth = health;
        
        UpdateHealthUI();
        //  Shop.instance.buySecondPosition += UpdateTimeBtwShoot;


    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && shootTimer >= timeBtwShoot){
            Shoot();
            shootTimer = 0;
        }

        shootSuperTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(1) && shootSuperTimer >= timeBtwSuperShoot && PlayerPrefs.GetInt("Position1") == 1){
            SuperShoot();
            shootSuperTimer = 0;
        }

        dashTimer += Time.deltaTime;

        dashSlider.value = dashTimer / timeBtwDash;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (dashTimer >= timeBtwDash)
            {
                dashTimer = 0;
                ActivateDash();
            }
        }
        
        if(timeBtwShoot - shootTimer < 0) return;
        text.text = ((int)((timeBtwShoot - shootTimer) * 100)/100f).ToString();
    }

    private void FixedUpdate()
    {
        Move();

        if(isDashing) Dash();
    }

    void UpdateTimeBtwShoot()
    {
        timeBtwShoot -= 0.5f;
        timeBtwSuperShoot -=0.8f;
    }

    #region Base Function

    void Dash()
    {
        rb.AddForce(moveInput * Time.fixedDeltaTime * dashForce * 100);
    }
    
    void ActivateDash()
    {
        isDashing = true;
        canBeDamage = false;
        
        Invoke(nameof(DeActivateDash), dashTime);
    }
    
    void DeActivateDash()
    {
        isDashing = false;
        canBeDamage = true;
    }
    
    void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(moveInput != Vector2.zero){
            anim.SetBool("Run", true);
            var emission = footParticle.emission;
            emission.rateOverTime = 10;
            footParticle.Pause();
            footParticle.Play();
        }
        else{
            anim.SetBool("Run", false);
            var emission = footParticle.emission;
            emission.rateOverTime = 0;
        }

        ScalePlayer(moveInput.x);

        moveVelocity = moveInput.normalized * speed;

        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    void ScalePlayer(float x)
    {
        if(x == 1){
            spR.flipX = false;
        }
        else if(x == -1){
            spR.flipX = true;
        }
    }


    #endregion

    void Shoot(){
        Instantiate(bullet, shootPos.position, shootPos.rotation);

        StartCoroutine(nameof(SetMuzzleFlash));
    }

    void SuperShoot()
    {
        for (int i = 0; i < shootSuperPos.Length; i++)
        {
            Instantiate(bullet, shootSuperPos[i].position, shootSuperPos[i].rotation);
        }
        CameraFollow.instance.CamShake();

        StartCoroutine(nameof(SetMuzzleFlash));
    }

    IEnumerator SetMuzzleFlash()
    {
        muzzleFlashSpR.enabled = true;
        muzzleFlashSpR.sprite = spritesMuzzleFlash[Random.Range(0, spritesMuzzleFlash.Length)];

        yield return new WaitForSeconds(0.1f);

        muzzleFlashSpR.enabled = false;
    }    
    
    public void Damage(int damage)
    {
        if(!canBeDamage) return;
        
        health -= damage;
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        
        CameraFollow.instance.CamShake();

        UpdateHealthUI();

        if (health <= 0)
        {
            deathPanel.SetActive(true);
            Destroy(gameObject);
        }
    }

    public void AddHealth(int value)
    {
        health  += value;
        if(health > maxHealth) health = maxHealth;

        UpdateHealthUI();
    }


    void UpdateHealthUI()
    {
        healthSlider.value = (float)health / maxHealth;
    }

    [HideInInspector] public int currentMoney;
    [SerializeField] private TextMeshProUGUI coinsText;

    public void AddMoney(int value)
    {
        currentMoney += value;
        coinsText.text = "Your Money: " + currentMoney.ToString();
    }
}

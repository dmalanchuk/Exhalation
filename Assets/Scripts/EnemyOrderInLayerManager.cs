using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOrderInLayerManager : MonoBehaviour
{

    public static EnemyOrderInLayerManager instance;

    private List<SpriteRenderer> enemyesSpR = new List<SpriteRenderer>();

    public void Add(SpriteRenderer spp)
    {
        enemyesSpR.Add(spp);
        
    }

    public void Dell(SpriteRenderer spp)
    {
        enemyesSpR.Remove(spp);
        
    }

    private float[] posYs;
    private SpriteRenderer[] spritesRends;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(nameof(Check));
    }

    IEnumerator Check()
    {
        yield return new WaitForSeconds(1);

        int n = enemyesSpR.Count;

        posYs = new float[n];
        spritesRends = new SpriteRenderer[n];

        for (int i = 0; i < n; i++)
        {
            posYs[i] = enemyesSpR[i].transform.position.y;
            spritesRends[i] = enemyesSpR[i];
        }
        
        Array.Sort(posYs,spritesRends);

        for (int i = 0; i < spritesRends.Length; i++)
        {
            spritesRends[i].sortingOrder = - i;
        }

        StartCoroutine(nameof(Check));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] float deathTime;

    private void Start()
    {
        Destroy(gameObject, deathTime);
    }
}

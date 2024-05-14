using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float minX, maxX, minY, maxY;
    [SerializeField] Transform target;
    [SerializeField] float followSpeed;

    private Animator anim;

    public static CameraFollow instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate(){
        if(!target) return;

        
        transform.position = Vector3.Lerp(transform.position,
        new Vector3(
            Mathf.Clamp(target.position.x, minX, maxX),
            Mathf.Clamp(target.position.y, minY, maxY),
        -10),
        followSpeed * Time.fixedDeltaTime);
        
        
        /*new Vector3(Mathf.Clamp(target.position.x, minX, maxX),
                                         Mathf.Clamp(target.position.y, minY, maxY),
        -10);*/

    }

    public void CamShake()
    {
        anim.Play("ShapeCamera");
    }
}

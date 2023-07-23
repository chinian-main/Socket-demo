using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteController: MonoBehaviour
{
    private Transform selfTransform;
    private Transform gunTransform;

    private Vector3 selfPos;
    private Quaternion selfAngle;
    private Quaternion gunAngle;

    public void SetState(Vector3 selfpos, float selfangle, float gunangle)
    {
        selfPos = selfpos;
        //直接用euler角[0,360],需要转换为四元数
        selfAngle=Quaternion.Euler(0,0,selfangle);
        gunAngle=Quaternion.Euler(0,0,gunangle);
    }
    
    private void Start()
    {
        selfTransform = transform;
        gunTransform = transform.Find("HandGun/Gun");
        selfAngle = selfTransform.rotation;
        selfPos = selfTransform.position;
        gunAngle = gunTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (selfTransform == null|| selfPos==null || gunTransform==null) return;
        selfTransform.position = Vector3.Lerp(selfTransform.position, selfPos, 0.25f);
        selfTransform.rotation = Quaternion.Slerp(selfTransform.rotation,selfAngle,0.25f);
        gunTransform.rotation = Quaternion.Slerp(gunTransform.rotation, gunAngle, 0.25f);
    }
}

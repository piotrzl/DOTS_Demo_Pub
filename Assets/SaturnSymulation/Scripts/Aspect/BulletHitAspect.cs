using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct BulletHitAspect : IAspect
{
    public readonly Entity entity;

    public readonly RefRW<LocalTransform> localTransform;


   
    public void HitEffectUpdate(float deltaTime) 
    {

    }
}

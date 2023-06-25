using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public readonly partial struct BulletAspect : IAspect
{
    public readonly Entity entity;

    public readonly RefRW<LocalTransform> localTransform;

    public readonly RefRO<BulletCpmponent> bulletComponent;

    public readonly RefRW<BulletFlyComponent> bulletFlyComponent;

    public void MoveForward(float deltaTime) 
    {
        float distance = deltaTime * bulletComponent.ValueRO.speed;
        bulletFlyComponent.ValueRW.currentDistance += distance;

        if(bulletFlyComponent.ValueRO.currentDistance > bulletComponent.ValueRO.range)
        {
            distance -= (bulletFlyComponent.ValueRO.currentDistance - bulletComponent.ValueRO.range);
            bulletFlyComponent.ValueRW.canFly = false;
        }    

        localTransform.ValueRW.Position += distance * bulletComponent.ValueRO.direction;
    }

    public float3 NextMove(float deltaTime) 
    {
       return localTransform.ValueRO.Position + deltaTime * bulletComponent.ValueRO.speed * bulletComponent.ValueRO.direction;
    }

    public bool CanMove() 
    {
    

        return bulletFlyComponent.ValueRO.canFly;
    }
}

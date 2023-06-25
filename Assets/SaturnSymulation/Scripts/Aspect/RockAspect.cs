using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Aspects;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UIElements;

public readonly partial struct RockAspect : IAspect
{
    public readonly Entity entity;

    public readonly RefRO<LocalTransform> localTransform;

    public readonly RefRO<RockComponent> rockComponent;

    public readonly RefRO<CircleMoveComponent> circleMoveComponent;

    public readonly RefRW<PhysicsVelocity> velocity;

    public readonly RefRO<PhysicsMass> mass;

   // public readonly RefRW<RigidBody>lol;
    public void CircleMove(float deltaTime, float3 center)
    {
    //    float speed = math.distance(float3.zero, velocity.ValueRO.Linear);

        float3 offset = localTransform.ValueRO.Position;
        
        float angle = math.atan2(offset.x, offset.z) + 1 * deltaTime;

        float3 targetPosition = center + new float3(math.sin(angle), 0, math.cos(angle)) * circleMoveComponent.ValueRO.targetRadius;
        targetPosition.y += circleMoveComponent.ValueRO.target_y;

        //float3 direction = targetPosition - localTransform.ValueRO.Position;

        float3 targetDirection = math.normalize(targetPosition - localTransform.ValueRO.Position);

        float targetSpeed = circleMoveComponent.ValueRO.targetSpeed;
        float dot = math.dot(targetDirection, math.normalize(velocity.ValueRO.Linear));
        if (dot > 0)
        {
            float curSpeed = math.length(velocity.ValueRO.Linear);
            targetSpeed = math.clamp(targetSpeed - curSpeed, 0, targetSpeed);
            /*
            if (curSpeed > targetSpeed)
            {
                targetSpeed = math.clamp(curSpeed - targetSpeed,0, targetSpeed);
            }
            */
        }
        velocity.ValueRW.ApplyLinearImpulse(mass.ValueRO, targetDirection * targetSpeed * deltaTime);
        /*
       float dot = math.dot(targetDirection, math.normalize(velocity.ValueRO.Linear));
        if (dot < 0)
        {
            dot *= -1f;
           // velocity.ValueRW.Linear += targetDirection * circleMoveComponent.ValueRO.targetSpeed * deltaTime * dot;
            velocity.ValueRW.ApplyLinearImpulse(mass.ValueRO, targetDirection * dot * circleMoveComponent.ValueRO.targetSpeed * deltaTime);
        }
        else if (math.distance(float3.zero, velocity.ValueRO.Linear) < circleMoveComponent.ValueRO.targetSpeed)
        {
           // velocity.ValueRW.Linear += targetDirection * circleMoveComponent.ValueRO.targetSpeed * deltaTime;
            velocity.ValueRW.ApplyLinearImpulse(mass.ValueRO, targetDirection * circleMoveComponent.ValueRO.targetSpeed * deltaTime);
        }
        /*
       float speed = math.distance(float3.zero, velocity.ValueRO.Linear);

        float3 offset = localTransform.ValueRO.Position - center;
        float angle = math.atan2(offset.x, offset.z) + circleMoveComponent.ValueRO.targetSpeed * deltaTime;
        float3 targetPosition = center + new float3(math.sin(angle), 0, math.cos(angle)) * circleMoveComponent.ValueRO.targetRadius;
        
        float3 direction = targetPosition - localTransform.ValueRO.Position;

       
        float dot = math.dot(math.normalize(direction), math.normalize(velocity.ValueRO.Linear)) * -1;
        dot = math.clamp(dot, 0, 1);
       
        if (speed != 0)
            direction -= direction * (math.clamp(speed, 0, circleMoveComponent.ValueRO.targetSpeed) / circleMoveComponent.ValueRO.targetSpeed);

        velocity.ValueRW.Linear += direction * dot;
        */
        // velocity.ValueRW.ApplyLinearImpulse(mass.ValueRO, direction);

    }

    public bool ToRmove() 
    {
        if (rockComponent.ValueRO.type == 0)
            return false;
                
        else return true;
    }
}



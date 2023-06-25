using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct SpaceShipAspect : IAspect
{
    public readonly Entity entity;
    public readonly RefRO<LocalTransform> localTransform;

    public readonly RefRW<PhysicsVelocity> velocity;

    public readonly RefRO<PhysicsMass> mass;

    public readonly RefRO<SpaceShipComponent> shipComponent;
    public void ShipConrtol(float mainThrusterPower,float3 rotation, float deltaTime) 
    {
        float mainPower = math.clamp(mainThrusterPower,0f, 1f);

        velocity.ValueRW.ApplyLinearImpulse(mass.ValueRO, localTransform.ValueRO.Forward() * mainPower * shipComponent.ValueRO.mainThrusterBust * deltaTime);

        float HorizontalPower = math.clamp(rotation.x, -1, 1f);
        float VerticalPower = math.clamp(rotation.y, -1, 1f);
        float rotateZPower = math.clamp(rotation.z, -1, 1f);

        float3 impulse = new float3(VerticalPower,HorizontalPower, rotateZPower) * shipComponent.ValueRO.rotationSpeed * deltaTime;

        velocity.ValueRW.ApplyAngularImpulse(mass.ValueRO, impulse);
    }

}

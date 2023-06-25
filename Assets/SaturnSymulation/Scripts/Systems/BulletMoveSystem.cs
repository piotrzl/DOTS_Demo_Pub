using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections.LowLevel.Unsafe;
//using static UnityEditor.ObjectChangeEventStream;
using Unity.Collections;
using UnityEngine.UIElements;
using Unity.Physics.Authoring;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct BulletMoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        var ECB = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();


        
        new MoveBulletJob
        {
            deltaTime = deltaTime,
            ECB = ECB.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            physicsWorld = physicsWorld.PhysicsWorld

        }.ScheduleParallel();

        
        


    }
}

[BurstCompile]
public partial struct MoveBulletJob : IJobEntity
{
    public float deltaTime;
    public EntityCommandBuffer.ParallelWriter ECB;
    [ReadOnly] public PhysicsWorld physicsWorld;

    //[NativeDisableUnsafePtrRestriction] public RefRW<PhysicsWorldSingleton> WorldSingleton;
    //  public CollisionWorld world;
    [BurstCompile]
    private void Execute(BulletAspect bullet, [EntityIndexInQuery]int sortKey)
    {
        float3 nextMove = bullet.NextMove(deltaTime);

        RaycastInput raycastInput = new RaycastInput { Start = bullet.localTransform.ValueRO.Position, End = nextMove, Filter = CollisionFilter.Default};

     //   RaycastHit raycastHit closestHit

        if (!physicsWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit raycastHit)) 
        {
            if (bullet.CanMove())
                bullet.MoveForward(deltaTime);
            else
                ECB.RemoveComponent<BulletFlyComponent>(sortKey, bullet.entity);
        }
        else 
        {
           

         //   raycastHit.e

            ECB.RemoveComponent<BulletFlyComponent>(sortKey, bullet.entity);
            ECB.AddComponent(sortKey, bullet.entity, new BulletHitComponent { hitPosition = raycastHit.Position});
        }

      
          //  ECB.DestroyEntity(sortKey,bullet.entity);
        //else
        // destroy


        //if(rock.ToRmove())

    }

    [BurstCompile]
    public partial struct HitBulletJob : IJobEntity
    {
        private void Execute(BulletHitAspect bullet)
        {

        }
    }

}
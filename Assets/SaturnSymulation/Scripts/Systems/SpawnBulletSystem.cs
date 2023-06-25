using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

//[UpdateInGroup(typeof(LateSimulationSystemGroup))]
//[UpdateAfter(typeof(PhysicsSystemGroup)))]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(BulletMoveSystem))]
//[UpdateAfter(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct SpawnBulletSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state) 
    {
        
        DynamicBuffer<BulletsIBufferData> bullets = SystemAPI.GetSingletonBuffer<BulletsIBufferData>();
        EntityCommandBuffer commandBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (DynamicBuffer<ShipShoot> shipShoots in SystemAPI.Query<DynamicBuffer<ShipShoot>>()) 
        {
            foreach(ShipShoot shot in shipShoots)
            { 
                Entity newBullet = commandBuffer.Instantiate(bullets[shot.bulletType].bulletPrefab);
                RaycastInput raycastInput = new RaycastInput {
                    Start = shot.position,
                    End = shot.position + shot.forward * bullets[shot.bulletType].bulletData.speed * deltaTime,
                    Filter = CollisionFilter.Default
                };

                float3 spawnPoint = shot.position;
                if (physicsWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit hit)) 
                {
                    commandBuffer.AddComponent(newBullet, new BulletHitComponent
                    {
                        hitPosition = hit.Position
                    });
                    spawnPoint = hit.Position;
                }
                else 
                {
                    commandBuffer.AddComponent(newBullet, new BulletFlyComponent
                    {
                        currentDistance = 0,
                        canFly = true
                    });
                }


                commandBuffer.SetComponent(newBullet, new LocalTransform
                {
                    Position = spawnPoint,
                    Rotation = quaternion.LookRotation(shot.forward, shot.up),
                    Scale = 1f,

                });

                commandBuffer.AddComponent(newBullet, new BulletCpmponent
                {
                    speed = bullets[shot.bulletType].bulletData.speed,
                    direction = shot.forward,
                    range = bullets[shot.bulletType].bulletData.range
                });

                //commandBuffer.SetComponent(newBullet, new Transla)
            }


            

            shipShoots.Clear();
        }
        
        //commandBuffer.Playback(state.EntityManager);
    }
}

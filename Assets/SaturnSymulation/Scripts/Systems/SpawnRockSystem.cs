using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static RockRegister;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnRockSystem : ISystem 
{
    public void OnCreate(ref SystemState state) 
    {
        state.RequireForUpdate<RockGeneratorComponent>();
    }


    [BurstCompile]
    public void OnUpdate(ref SystemState state) 
    {
      

        Entity rockGeneratorEntity = SystemAPI.GetSingletonEntity<RockGeneratorComponent>();
        RockGeneratorAspect rockGeneratorAspect = SystemAPI.GetAspect<RockGeneratorAspect>(rockGeneratorEntity);

        if (!rockGeneratorAspect.CanSpawn())
            state.Enabled = false;



        EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.Temp);

        DynamicBuffer<RocksIBufferData> rocks = SystemAPI.GetSingletonBuffer<RocksIBufferData>();

        //rockGeneratorAspect._rockGeneratorComponent.ValueRO.rockCount
  
        // spawn rock
        for (int i = 0; i < rockGeneratorAspect._rockGeneratorComponent.ValueRO.spawnRockCountPerFrame; ++i) 
        {
            Entity newRock = commandBuffer.Instantiate(rocks[rockGeneratorAspect.GetRandomNum(0,rocks.Length)].rockPrefab);

            float3 randomPoint = rockGeneratorAspect.GetRandomPosition();
            float randomScale = rockGeneratorAspect.GetRandomScale();

         //   Debug.Log((int)randomScale);

            commandBuffer.SetComponent(newRock, new LocalTransform
            {
                Position = randomPoint,
                Rotation = quaternion.identity,
                Scale = randomScale
            });

          
            commandBuffer.SetComponent(newRock, new CircleMoveComponent
            {
                targetRadius = math.distance(randomPoint, rockGeneratorAspect._localTransform.ValueRO.Position),
                targetSpeed = rockGeneratorAspect.GetRandomRockSpeed(),
                target_y = randomPoint.y
            });

            rockGeneratorAspect.AddRock();
             if (!rockGeneratorAspect.CanSpawn())
                    break;

            
        }
        commandBuffer.Playback(state.EntityManager);

            

        
        //commandBuffer.Playback(state.EntityManager);
    }
}

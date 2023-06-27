using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct MoveRockSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float delaTime = SystemAPI.Time.DeltaTime;


     //   Entity rockGeneratorEntity = SystemAPI.GetSingletonEntity<RockGeneratorComponent>();
     //   RockGeneratorAspect rockGeneratorAspect = SystemAPI.GetAspect<RockGeneratorAspect>(rockGeneratorEntity);

        Entity saturnEntity = SystemAPI.GetSingletonEntity<SaturnROComponent>();
        SaturnAspect saturnAspect = SystemAPI.GetAspect<SaturnAspect>(saturnEntity);

        new MoveRockJob
        {
            deltaTime = delaTime,
            center = saturnAspect._localTransform.ValueRO.Position,
        }.ScheduleParallel();


        //foreach (var brain in SystemAPI.Query<>())
        //{
        //    brain.DamageBrain();
        //}


    }
}
[BurstCompile]
public partial struct MoveRockJob : IJobEntity 
{
    public float deltaTime;
    public float3 center;

    [BurstCompile]
    private void Execute(RockAspect rock) 
    {
        rock.CircleMove(deltaTime, center);



        //if(rock.ToRmove())

    }
}
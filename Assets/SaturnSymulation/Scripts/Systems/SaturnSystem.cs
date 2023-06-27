using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct SaturnSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        /*
        float delaTime = SystemAPI.Time.DeltaTime;


        Entity saturnEntity = SystemAPI.GetSingletonEntity<SaturnROComponent>();
        SaturnAspect saturnAspect = SystemAPI.GetAspect<SaturnAspect>(saturnEntity);
        */
    }

}

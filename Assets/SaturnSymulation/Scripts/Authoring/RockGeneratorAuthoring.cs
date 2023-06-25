using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class RockGeneratorAuthoring : MonoBehaviour
{
    [Header("Rock Count")]
    [Min(1)] public int rockCount = 10000;
    [Min(1)] public int spawnRockCountPerFrame = 50;

    [Header("Rock Scale")]
    [Min(0.001f)] public float minRockScale = 1f;
    public float rangeRockScale = 50f;
    [Range(0,1)] public float popularValue = 0.1f;

    [Header("Rock Speed")]
    public float minRockSpeed = 100f;
    public float rangeRockSpeed = 250f;

    [Header("Rock Range")]
    public float minSpawnRange = 8000f;
    public float ringWeight = 12000f;
    public float ringHight;

    [Header("Random Value")]
    public uint RandomSeed;
}

public struct RockGeneratorComponent : IComponentData
{
    public int rockCount;
    public int spawnRockCountPerFrame;

    public float minRockScale;
    public float rangeRockScale;
    public float popularValue;

    public float minRockSpeed;
    public float bonusRockSpeed;

    public float minSpawnRange;
    public float ringWeight;
    public float ringHight;
}

public class RockGeneratorBaker : Baker<RockGeneratorAuthoring>
{
    [System.Obsolete]
    public override void Bake(RockGeneratorAuthoring authoring)
    {
     
        AddComponent(new RockGeneratorComponent() {
            rockCount = authoring.rockCount,
            spawnRockCountPerFrame = authoring.spawnRockCountPerFrame,
            minRockScale = authoring.minRockScale,
            rangeRockScale = authoring.rangeRockScale,
            popularValue = authoring.popularValue,

            minRockSpeed = authoring.minRockSpeed,
            bonusRockSpeed = authoring.rangeRockSpeed,

            minSpawnRange = authoring.minSpawnRange,
            ringWeight = authoring.ringWeight,
            ringHight = authoring.ringHight,
        });

        AddComponent(new RandomGenerator
        {
            rockCount = 0,
            Value = Unity.Mathematics.Random.CreateFromIndex(authoring.RandomSeed)
        }) ;
    }
}

public struct RandomGenerator : IComponentData 
{
    public int rockCount;
    public Unity.Mathematics.Random Value;
}

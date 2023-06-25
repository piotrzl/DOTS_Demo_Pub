using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static RockRegister;

public readonly partial struct RockGeneratorAspect : IAspect
{
    public readonly Entity entity;

    public readonly RefRW<LocalTransform> _localTransform;

    public readonly RefRO<RockGeneratorComponent> _rockGeneratorComponent;
    public readonly RefRW<RandomGenerator> _randomGenerator;


    public float GetRandomRockSpeed() 
    {
        return _randomGenerator.ValueRW.Value.NextFloat(
            _rockGeneratorComponent.ValueRO.minRockSpeed,
            _rockGeneratorComponent.ValueRO.minRockSpeed + _rockGeneratorComponent.ValueRO.bonusRockSpeed
            );
    }

    public float3 GetRandomPosition() 
    {
        float3 center = _localTransform.ValueRO.Position;

        // Direcion form center 
        float2 ringPosition = _randomGenerator.ValueRW.Value.NextFloat2(new float2(-1,-1), new float2(1,1));

        ringPosition = math.normalize(ringPosition);

        // set position
        ringPosition *= _randomGenerator.ValueRW.Value.NextFloat(_rockGeneratorComponent.ValueRO.minSpawnRange, _rockGeneratorComponent.ValueRO.minSpawnRange + _rockGeneratorComponent.ValueRO.ringWeight);

        float ringHight = _randomGenerator.ValueRW.Value.NextFloat(-_rockGeneratorComponent.ValueRO.ringHight/2, _rockGeneratorComponent.ValueRO.ringHight/2);

        float3 randomPoint = center + new float3(ringPosition.x, ringHight, ringPosition.y);

        return randomPoint;
    }

    public float GetRandomScale() 
    {
        bool anmomaly = _randomGenerator.ValueRW.Value.NextInt(0,1000) == 1;
        

        float standardDeviationFactor = 0.1f;

        float max = _rockGeneratorComponent.ValueRO.minRockScale + _rockGeneratorComponent.ValueRO.rangeRockScale;

        float range = _rockGeneratorComponent.ValueRO.rangeRockScale;

        float mostPopularValue;
        if (anmomaly)
            mostPopularValue = _rockGeneratorComponent.ValueRO.minRockScale + range * (1 - _rockGeneratorComponent.ValueRO.popularValue);
        else
             mostPopularValue = _rockGeneratorComponent.ValueRO.minRockScale + range * _rockGeneratorComponent.ValueRO.popularValue;

        float standardDeviation = range * standardDeviationFactor;
        float value = NextGaussian(mostPopularValue, standardDeviation);
        return math.clamp(value, _rockGeneratorComponent.ValueRO.minRockScale, max);



        /*
        float popularScale = 5f;

        float randomScale = _randomGenerator.ValueRW.Value.NextFloat(
          _rockGeneratorComponent.ValueRO.minRockScale,
          _rockGeneratorComponent.ValueRO.minRockScale + _rockGeneratorComponent.ValueRO.bonusRockScale
          );



        return _randomGenerator.ValueRW.Value.NextFloat(
          _rockGeneratorComponent.ValueRO.minRockScale,
          _rockGeneratorComponent.ValueRO.minRockScale + _rockGeneratorComponent.ValueRO.bonusRockScale
          );
        */
    }

    float GetRandom(float min, float max, float popular)
    {
        float range = max - min;
        float mostPopularValue = min + range * popular;
        float standardDeviation = range / 2;
        float value = NextGaussian(mostPopularValue, standardDeviation);
        return math.clamp(value, min, max);
    }

    float NextGaussian(float mean, float standardDeviation)
    {
        float v1, v2, s;
        do
        {
            v1 = 2.0f * _randomGenerator.ValueRW.Value.NextFloat(0f, 1f) - 1.0f;
            v2 = 2.0f * _randomGenerator.ValueRW.Value.NextFloat(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);

        s = math.sqrt((-2.0f * math.log(s)) / s);

        return mean + standardDeviation * v1 * s;
    }



    public int GetRandomNum(int min, int max) 
    {
        int random = _randomGenerator.ValueRW.Value.NextInt(min, max);

        return random;
    }


    public void AddRock() 
    {
        _randomGenerator.ValueRW.rockCount++;
    }

    public int GetRockCount() 
    {
        return _randomGenerator.ValueRO.rockCount;
    }

    public bool CanSpawn() 
    {
        return _randomGenerator.ValueRO.rockCount < _rockGeneratorComponent.ValueRO.rockCount;
    }

}

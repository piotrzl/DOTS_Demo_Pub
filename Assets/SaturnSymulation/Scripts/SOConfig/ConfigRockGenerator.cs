using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RockGeneratorConfig", menuName = "Config/RockGeneratorConfig")]
public class ConfigRockGenerator : ScriptableObject
{
    [Header("Rock Count")]
    [Min(1)] public int rockCount = 10000;
    [Min(1)] public int spawnRockCountPerFrame = 50;

    [Header("Rock Scale")]
    [Min(0.001f)] public float minRockScale = 1f;
    public float rangeRockScale = 50f;
    [Range(0, 1)] public float popularValue = 0.1f;

    [Header("Rock Speed")]
    public float minRockSpeed = 100f;
    public float rangeRockSpeed = 250f;

    [Header("Rock Range")]
    public float minSpawnRange = 9000f;
    public float ringWeight = 14000f;
    public float ringHight = 1200;
}

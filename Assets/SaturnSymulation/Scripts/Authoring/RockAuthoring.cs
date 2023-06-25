using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class RockAuthoring : MonoBehaviour
{
    public int type;
}

public struct RockComponent : IComponentData
{
    public int type;
}
public class RockBaker : Baker<RockAuthoring>
{
    [System.Obsolete]
    public override void Bake(RockAuthoring authoring)
    {
        AddComponent(new RockComponent() {
            type = authoring.type,
        });

        AddComponent(new CircleMoveComponent { });
    }
}

public struct CircleMoveComponent : IComponentData 
{
    public float targetRadius;
    public float targetSpeed;
    public float target_y;
}


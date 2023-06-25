using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerTagAuthoring : MonoBehaviour
{
    
}

public class PlayerTagBaker : Baker<PlayerTagAuthoring>
{
    [System.Obsolete]
    public override void Bake(PlayerTagAuthoring authoring)
    {
        AddComponent(new PlayerTagComponent { });
    }

}

public struct PlayerTagComponent : IComponentData
{

}
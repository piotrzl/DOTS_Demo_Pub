using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class SaturnAuthoring : MonoBehaviour
{
}

public class SaturnBaker : Baker<SaturnAuthoring>
{
    [System.Obsolete]
    public override void Bake(SaturnAuthoring authoring)
    {
        AddComponent(new SaturnROComponent()
        {

        });
    }
}

public struct SaturnROComponent : IComponentData
{

}
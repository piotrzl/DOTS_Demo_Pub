using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SpaceShipAuthoring : MonoBehaviour
{
    public float mainThrusterBust;
    public float rotationSpeed;
}

public class SpaceShipBaker : Baker<SpaceShipAuthoring>
{
    [System.Obsolete]
    public override void Bake(SpaceShipAuthoring authoring)
    {
        AddComponent(new SpaceShipComponent {
            mainThrusterBust = authoring.mainThrusterBust, 
            rotationSpeed = authoring.rotationSpeed
        });
    }

}

public struct SpaceShipComponent : IComponentData
{
    public float mainThrusterBust;
    public float rotationSpeed;
}
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class BulletRgister : MonoBehaviour
{
    //public List<GameObject> bullets;
    public List<BulletStats> bullets;
    class Baker : Baker<BulletRgister> 
    {
        [System.Obsolete]
        public override void Bake(BulletRgister authoring) 
        {
            var Buffer = AddBuffer<BulletsIBufferData>();
            foreach(BulletStats bullet in authoring.bullets) 
            {
                Buffer.Add(new BulletsIBufferData
                {
                    bulletPrefab = GetEntity(bullet.gameObject),
                    bulletData = new BulletCpmponent 
                    {
                        speed = bullet.speed,
                        range = bullet.range
                    }
                });
            }
        }
    }
}

public struct BulletsIBufferData : IBufferElementData 
{
    public Entity bulletPrefab;
    public BulletCpmponent bulletData;
}

public struct BulletCpmponent : IComponentData 
{
    public float speed;
    public float3 direction;
    public float range;
}

public struct BulletFlyComponent : IComponentData
{
    public float currentDistance;
    public bool canFly;
}

public struct BulletHitComponent : IComponentData
{
    public float3 hitPosition;
}
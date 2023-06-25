using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class SpaceShip : MonoBehaviour
{
    public UnityEvent SpaceShipIsUpdateEvent;
 //   public UnityEvent<float> AvarageSpeedUpdateEvent;

    Entity spaceShipEntity;
    Entity entity;
    public GameObject laserCanon_1;
    public GameObject laserCanon_2;
    World world;
    [SerializeField] EffectControler effectControler;
    void OnEnable()
    {
        spaceShipEntity = FindShipEntity();
        world = World.DefaultGameObjectInjectionWorld;

        effectControler.SetThrusterPower(1);
    }

    // Update is called once per frame

    void LateUpdate()
    {
        
        
        // fallow entity ship 
        if (spaceShipEntity != Entity.Null)
        {
            Vector3 position = world.EntityManager.GetComponentData<LocalToWorld>(spaceShipEntity).Position;
            transform.position = position;
            Quaternion rotation = world.EntityManager.GetComponentData<LocalToWorld>(spaceShipEntity).Rotation;
            transform.rotation = rotation;
        }
        else
        {
            spaceShipEntity = FindShipEntity();
        }
        SpaceShipIsUpdateEvent?.Invoke();
    }
   



    public void SetThrusterPower(int power) 
    {
        if (!effectControler)
            return;

        effectControler.SetThrusterPower(power);
    }

    public void EnableThruster(bool enable) 
    {
        if (!effectControler)
            return;

        effectControler.EnableTrialThruster(enable);
        effectControler.EnableParticleThruster(enable);
    }
    

    public void ShootLaser() 
    {

        SetEntity();

        world.EntityManager.GetBuffer<ShipShoot>(entity).Add(new ShipShoot
        {
            bulletType = 0,
            position = laserCanon_1.transform.position,
            forward = laserCanon_1.transform.forward,
            up = laserCanon_1.transform.up,
            
        });
        world.EntityManager.GetBuffer<ShipShoot>(entity).Add(new ShipShoot
        {
            bulletType = 0,
            position = laserCanon_2.transform.position,
            forward = laserCanon_2.transform.forward,
            up = laserCanon_2.transform.up,
        });
    }
   
    void SetEntity()
    {
        if (world.IsCreated && !world.EntityManager.Exists(entity))
        {
            entity = world.EntityManager.CreateEntity();
            world.EntityManager.AddBuffer<ShipShoot>(entity);
        }
    }

    Entity FindShipEntity() 
    {
        EntityQuery playerQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(PlayerTagComponent));
        NativeArray<Entity> entityNativeArray = playerQuery.ToEntityArray(Allocator.Temp);
        if (entityNativeArray.Length > 0)
        {
            return entityNativeArray[UnityEngine. Random.Range(0, entityNativeArray.Length)];
        }
        else
            return Entity.Null;
    }

    void OnDestroy()
    {
        
        if (world.IsCreated && world.EntityManager.Exists(entity))
        {
            world.EntityManager.DestroyEntity(entity);
           // world.EntityManager.AddBuffer<ShipShoot>(entity);
        }
        
    }
}

public struct ShipShoot : IBufferElementData 
{
    public int bulletType;
    public float3 position;
    public float3 forward;
    public float3 up;
}


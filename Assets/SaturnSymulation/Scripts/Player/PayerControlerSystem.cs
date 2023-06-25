using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;


//[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial class PayerControlerSystem : SystemBase
{
    int currentPower = 1;

    protected override void OnUpdate()
    {
        Entity playerShipEntity = SystemAPI.GetSingletonEntity<PlayerTagComponent>();
        SpaceShipAspect spaceShipAspect = SystemAPI.GetAspect<SpaceShipAspect>(playerShipEntity);

        float3 rotation = float3.zero;
        rotation.x = Input.GetAxis("Horizontal");
        rotation.y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.E))
            rotation.z = -1;
        else if (Input.GetKey(KeyCode.Q))
            rotation.z = 1;

        if (Input.inputString != "")
        {
            //int number;
            bool is_a_number = Int32.TryParse(Input.inputString, out int number);
            if (is_a_number && number >= 0 && number < 10)
            {
                if (number == 0)
                    number = 10;

                currentPower = number;
            }
        }
        float mainThruster = 0;
        if (Input.GetButton("Jump"))
            mainThruster = 0.1f * currentPower;

        spaceShipAspect.ShipConrtol(mainThruster,rotation, SystemAPI.Time.DeltaTime);



    }
}

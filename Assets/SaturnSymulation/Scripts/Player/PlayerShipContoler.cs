using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShipContoler : MonoBehaviour
{
    public UnityEvent<float> SpaceShipSpeedUpdateEvent;
    public UnityEvent<int> SpaceShipThrusterPowerUpdateEvent;

    [SerializeField] SpaceShip _spaceShip;
    [SerializeField] Camera _camera;
    [Header("Camera offSet")]
    [SerializeField] Vector3 cameraMaxBonusDistance;
    [SerializeField] float minSppeed = 100f;
    [SerializeField] float maxSpeed = 500f;
    [SerializeField] float cameraRotationStrenght = 8f;

    Vector3 cameraBaseLocalPosition;
    float currentSpeed = 0;
    Vector3 cameraVelocity;
    // last
    Vector3 lastShipPosition;
    float time = 0;
    float distance = 0;

    void OnEnable()
    {
        _camera = Camera.main;
        _camera.transform.SetParent(transform);
        cameraBaseLocalPosition = _camera.transform.localPosition;

        _spaceShip.SpaceShipIsUpdateEvent.AddListener(UpdateAfterShip);

        ShipUI shipUI = FindAnyObjectByType<ShipUI>();

        shipUI.AddShippToUpdate(this);

       // shipUI.UpdatePowerText


        // lastShipPosition = _spaceShip.transform.position;
    }

    private void OnDisable()
    {
        if(_spaceShip)
        _spaceShip.SpaceShipIsUpdateEvent.AddListener(UpdateAfterShip);
    }

    void Update()
    {
        if (UnityEngine.Input.GetMouseButtonDown(0))
            _spaceShip.ShootLaser();

        if (Input.GetButtonDown("Jump"))
            _spaceShip.EnableThruster(true);
        else if (Input.GetButtonUp("Jump"))
            _spaceShip.EnableThruster(false);

        if (Input.inputString != "")
        {
            bool is_a_number = Int32.TryParse(Input.inputString,out int number);
            if (is_a_number && number >= 0 && number < 10)
            {
                if (number == 0)
                    number = 10;

                _spaceShip.SetThrusterPower(number);
                SpaceShipThrusterPowerUpdateEvent?.Invoke(number * 10);
            }
        }

    }

    void UpdateAfterShip()
    {
        time += Time.deltaTime;
        distance += Vector3.Distance(_spaceShip.transform.position, lastShipPosition);

        if (time != 0f)
           currentSpeed = distance / time;
        else
            currentSpeed = 0f;

        if (time >= 1f) 
        {
            currentSpeed = distance / time;
            SpaceShipSpeedUpdateEvent?.Invoke(currentSpeed);
            time = 0;
            distance = 0;
        }
       

        CameraUpdate();
        lastShipPosition = _spaceShip.transform.position;
    }

   
    void CameraUpdate() 
    {
        Vector3 currentTargetPosition = cameraBaseLocalPosition + cameraMaxBonusDistance * Mathf.InverseLerp(minSppeed, maxSpeed, currentSpeed);
        Vector3 finalPosition = Vector3.SmoothDamp(_camera.transform.localPosition,currentTargetPosition,ref cameraVelocity, 0.5f);


        Vector3 rotation = Vector3.zero;
        rotation.x = Input.GetAxis("Horizontal");
        rotation.y = -Input.GetAxis("Vertical");
        finalPosition = Vector3.LerpUnclamped(finalPosition, finalPosition + rotation * cameraRotationStrenght, Time.deltaTime/2f);

        _camera.transform.localPosition = finalPosition; // Vector3.LerpUnclamped(_camera.transform.localPosition,currentTargetPosition, Time.deltaTime);

        
        
        _camera.transform.LookAt(_spaceShip.transform.position + _spaceShip.transform.up * 0.11f, _spaceShip.transform.up);
    }
}

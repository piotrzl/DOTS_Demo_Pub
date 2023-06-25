using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShipUI : MonoBehaviour
{

    [SerializeField] TMP_Text powerText;
    
    [SerializeField] TMP_Text speedText;
    [SerializeField] PlayerShipContoler _playerShipContoler;
    public void AddShippToUpdate(PlayerShipContoler playerShipContoler) 
    {
        if (!playerShipContoler)
            return;

        if (_playerShipContoler)
        {
            _playerShipContoler.SpaceShipThrusterPowerUpdateEvent.RemoveListener(UpdatePowerText);
            _playerShipContoler.SpaceShipSpeedUpdateEvent.RemoveListener(UpdateSpeedText);
        }

        _playerShipContoler = playerShipContoler;
        _playerShipContoler.SpaceShipThrusterPowerUpdateEvent.AddListener(UpdatePowerText);
        _playerShipContoler.SpaceShipSpeedUpdateEvent.AddListener(UpdateSpeedText);
    }

    public void UpdatePowerText(int powerLvl) 
    {
        powerText.text = "Thruster Power: " + powerLvl.ToString() + " %";
    }

    public void UpdateSpeedText(float speed) 
    {
        speedText.text = "Speed: " + ((int)speed).ToString() + " m/s";
    }


    private void OnEnable()
    {
        if (_playerShipContoler)
        {
            _playerShipContoler.SpaceShipThrusterPowerUpdateEvent.AddListener(UpdatePowerText);
            _playerShipContoler.SpaceShipSpeedUpdateEvent.AddListener(UpdateSpeedText);
        }
    }


    private void OnDisable()
    {
        if (_playerShipContoler)
        {
            _playerShipContoler.SpaceShipThrusterPowerUpdateEvent.RemoveListener(UpdatePowerText);
            _playerShipContoler.SpaceShipSpeedUpdateEvent.RemoveListener(UpdateSpeedText);
        }
    }
}

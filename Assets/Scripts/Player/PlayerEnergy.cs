using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{

    [Header("Player Energy Settings")]

    [Tooltip("The total health of the player")]
    [SerializeField] private float _playerEnergyMax;

    [Tooltip("The amount of energy lost per second")]
    [SerializeField] private float _playerEnergyDecay;

    private float _playerEnergy = 0;
    public float PlayerEnergyAmount
    {
        get { return _playerEnergy; }
        set
        {
            _playerEnergy = value;
            Mathf.Clamp(_playerEnergy, 0f, _playerEnergyMax);
        }
    }
    
    /// <summary>
    /// Hard coding the start player energy amount to the max amount
    /// </summary>
    void Start()
    {
        // Todo: Assess - do we want to always have max energy when starting a game?
        PlayerEnergyAmount = _playerEnergyMax;
    }

    /// <summary>
    /// Applying decay amount to the player energy
    /// </summary>
    void Update()
    {
        PlayerEnergyAmount -= _playerEnergyDecay * Time.deltaTime;
        Debug.Log("PlayerEnergyAmount: " + PlayerEnergyAmount);
    }
}

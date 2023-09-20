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
            _playerEnergy = Mathf.Clamp(_playerEnergy, 0f, _playerEnergyMax);
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
    }

    /// <summary>
    /// When the player collides with something, if it's a food thing, the player takes damage, becomes invincible, and flashes
    /// Triggers the game over state if _layerHealth is <= 0
    ///     *game over state not yet implemented
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food") == true)
        {
            PlayerFoundFood(collision);
        }
    }

    void PlayerFoundFood(Collider2D collision)
    {
        _playerEnergy += collision.gameObject.GetComponent<Projectile>().GetProjectileValue();
        Debug.Log("Food was found! Player energy: " + _playerEnergy);
        // Set the object hitting the player to inactive (spawner should take care of it from here)
        collision.gameObject.SetActive(false);
    }

}

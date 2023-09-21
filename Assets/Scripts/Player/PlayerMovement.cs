using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Settings")]

    [Tooltip("Determines speed of player movement")]
    [SerializeField] private float _playerMoveSpeed;

    [Tooltip("Multiplier for movement during the dodge")]
    [SerializeField] private float _playerDodgeMultiplier;
    
    [Tooltip("Time spent doing the dodge (invincible for this amount of time)")]
    [SerializeField] private float _playerDodgeTime;

    [Tooltip("Cost for doing the dodge")]
    [SerializeField] private float _playerDodgeCost;

    [Tooltip("The Rigidbody 2D component of the player object")]
    [SerializeField] private Rigidbody2D _playerRigidbody;

    private Vector2 _movement;
    private bool _bDodging = false;
    private PlayerHealth playerHealth;
    private PlayerEnergy playerEnergy;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null)
            Debug.LogError("Couldn't grab the player health component");

        playerEnergy = GetComponent<PlayerEnergy>();
        if (playerHealth == null)
            Debug.LogError("Couldn't grab the player energy component");
    }

    void Update()
    {
        if (_bDodging)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && playerEnergy.PlayerEnergyAmount >= _playerDodgeCost)
        {
            DoDodge();
            return;
        }

        _movement.x = Input.GetAxis("Horizontal");
        _movement.y = Input.GetAxis("Vertical");
        _movement.Normalize();
    }

    /// <summary>
    /// SW| Gets the directional input and updates the player object's position based on _moveSpeed
    /// </summary>
    void FixedUpdate()
    {
        _playerRigidbody.AddForce(_movement * _playerMoveSpeed, ForceMode2D.Force);
    }

    void DoDodge()
    {
        _bDodging = true;
        playerHealth.IsInvincible = true;
        playerEnergy.PlayerEnergyAmount -= _playerDodgeCost;

        _movement *= _playerDodgeMultiplier;

        StartCoroutine(EDodgingTimer());
    }

    /// <summary>
    /// Letting dodge velocity play out for _playerDodgeTime and then turns off the dodging effect
    /// </summary>
    /// <returns></returns>
    IEnumerator EDodgingTimer()
    {
        yield return new WaitForSeconds(_playerDodgeTime);

        _bDodging = false;
        playerHealth.IsInvincible = false;
    }
}

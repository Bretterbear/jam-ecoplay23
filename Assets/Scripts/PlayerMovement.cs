using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Settings")]

    [Tooltip("Determines speed of player movement")]
    [SerializeField] private float _playerMoveSpeed;

    [Tooltip("The Rigidbody 2D component of the player object")]
    [SerializeField]private Rigidbody2D _playerRigidbody;

    private Vector2 _movement;
  
    /// <summary>
    /// SW| Gets the directional input and updates the player object's position based on _moveSpeed
    /// </summary>
    void FixedUpdate()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        _movement.Normalize();
        _playerRigidbody.MovePosition(_playerRigidbody.position + _movement * _playerMoveSpeed * Time.fixedDeltaTime);
    }
}

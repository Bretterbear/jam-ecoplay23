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
    
    [Tooltip("Multiplier for movement during the dodge")]
    [SerializeField] private float _playerDodgeTime;

    [Tooltip("The Rigidbody 2D component of the player object")]
    [SerializeField] private Rigidbody2D _playerRigidbody;

    private Vector2 _movement;
    private bool _bDodging = false;

    void Update()
    {
        if (_bDodging)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoDodge();
            return;
        }

        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        _movement.Normalize();
    }

    /// <summary>
    /// SW| Gets the directional input and updates the player object's position based on _moveSpeed
    /// </summary>
    void FixedUpdate()
    {
        _playerRigidbody.velocity = _movement * _playerMoveSpeed * Time.fixedDeltaTime;
    }

    void DoDodge()
    {
        _bDodging = true;

        Vector2 mouseScreenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition -= _playerRigidbody.position;
        mouseWorldPosition.Normalize();
        _movement = mouseWorldPosition;

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
    }
}

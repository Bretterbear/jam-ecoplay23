using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;

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

    [Tooltip("Determines speed of player movement")]
    [SerializeField] private bool _bFaceDirection = false;

    [Tooltip("How slowly should we turn (default 0.1)")]
    [SerializeField] private float _torqueAmount = 0.1f;

    [Tooltip("The Rigidbody 2D component of the player object")]
    [SerializeField] private Rigidbody2D _playerRigidbody;

    [Tooltip("The Polygon Collider 2D component of the player object")]
    [SerializeField] private Collider2D _playerCollider;

    private SpriteRenderer _playerSprite;
    private Color _normalColor;
    private Vector2 _movement;
    private bool _bDodging = false;
    private PlayerHealth playerHealth;
    private PlayerEnergy playerEnergy;

    void Start()
    {
        _playerSprite = this.GetComponentInChildren<SpriteRenderer>();
        _playerCollider = this.GetComponent<Collider2D>();
        _normalColor = _playerSprite.color;

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

        // Turned off by default, but something to try if you want
        if (_bFaceDirection && _movement.magnitude > 0.1f)
        {

            float rotation = _playerRigidbody.rotation % 360;
            Vector2 lookDirection = _playerRigidbody.transform.right;

            Vector3 cross = Vector3.Cross(_movement, new Vector2(1, 0));
            float sign = Mathf.Sign(cross.z);

            float angle = Vector2.Angle(_movement, new Vector2(1, 0));
            angle *= sign* -1;

            float final = (angle - rotation) * _torqueAmount + rotation;
            _playerRigidbody.MoveRotation(final);
        }
    }

    void DoDodge()
    {
        _bDodging = true;
        playerHealth.IsInvincible = true;
        _playerCollider.enabled = false;
        StartCoroutine(EInvincibleFlash());
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
        _playerCollider.enabled = true;
    }

    IEnumerator EInvincibleFlash()
    {
        for (int i = 0; i < 3; i++) 
        {
            _playerSprite.color = new Color(1f, 0.92f, 0.016f, 1f);
            yield return new WaitForSeconds(_playerDodgeTime/6f);
            _playerSprite.color = _normalColor;
            yield return new WaitForSeconds(_playerDodgeTime/6f);
        }
    }
}

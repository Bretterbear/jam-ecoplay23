using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [Header("Player Health Settings")]

    [Tooltip("The total health of the player")]
    [SerializeField] private float _playerHealth;

    [Tooltip("The player's sprite renderer")]
    [SerializeField] private SpriteRenderer _playerSprite;

    private Color _normalColor;

    private bool _bInvincible = false;
    public bool IsInvincible
    {
        get { return _bInvincible; }
        set { _bInvincible = value; }
    }

    /// <summary>
    /// Grabs the normal color of the player sprite to use for damage flash
    /// </summary>
    void Start()
    {
        _normalColor = _playerSprite.color;
    }

    /// <summary>
    /// When the player collides with something, if it's a bullet, the player takes damage, becomes invincible, and flashes
    /// Triggers the game over state if _layerHealth is <= 0
    ///     *game over state not yet implemented
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_bInvincible == false 
            && (collision.gameObject.CompareTag("Bullet") == true || collision.gameObject.CompareTag("Enemy") == true))
        {
            PlayerHasBeenHit(collision);
        }

        CheckPlayerHealth();
    }

    void PlayerHasBeenHit(Collider2D collision)
    {
        _bInvincible = true;
        _playerHealth += collision.gameObject.GetComponent<Projectile>().GetProjectileValue();
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/BulletDamage");

        // Set the object hitting the player to inactive (spawner should take care of it from here)
        collision.gameObject.SetActive(false);
        StartCoroutine(EDamageFlash());
    }

    void CheckPlayerHealth()
    {
        if (_playerHealth <= 0f)
        {
            // Will trigger game over state
            gameObject.SetActive(false);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Die");
        }
    }

    /// <summary>
    /// Makes the player flash for 2 seconds, the become not invincible
    /// </summary>
    /// <returns></returns>
    IEnumerator EDamageFlash()
    {
        for (int i = 0; i < 10; i++) 
        {
            _playerSprite.color = new Color(0, 0, 0, 1);
            yield return new WaitForSeconds(0.1f);
            _playerSprite.color = _normalColor;
            yield return new WaitForSeconds(0.1f);
        }
        _bInvincible = false;
    }
}

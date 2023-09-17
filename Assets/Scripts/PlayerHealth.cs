using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [Header("Player Health Settings")]

    [Tooltip("The total health of the player")]
    [SerializeField] private float _playerHealth = 10f;

    [Tooltip("The player's sprite renderer")]
    [SerializeField] private SpriteRenderer _playerSprite;

    private Color _normalColor;

    private bool _bInvincible = false;

    /// <summary>
    /// Grabs the normal color of the player sprite to use for damage flash
    /// </summary>
    void Start()
    {
        _normalColor = _playerSprite.color;
    }

    /// <summary>
    /// When the player collides with somthing, if it's a bullet, the player takes damage, becomes invincible, and flashes
    ///     *game over state not yet imlemented
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet") == true && _bInvincible == false)
        {
            _bInvincible = true;
            _playerHealth = _playerHealth - collision.gameObject.GetComponent<Bullet>().GetDamage();
            StartCoroutine(EDamageFlash());
            if (_playerHealth <= 0f)
            {
                //Will trigger game over state
                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Makes the player flash for 2 seconds
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

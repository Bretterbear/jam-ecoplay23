using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    [Header("Player Health Settings")]

    [Tooltip("The total health of the player")]
    [SerializeField] private float _playerHealth;

    private SpriteRenderer _playerSprite;

    private Color _normalColor;

    private bool _bInvincible = false;
    public bool IsInvincible
    {
        get { return _bInvincible; }
        set { _bInvincible = value; }
    }

    private GameManagerService _linkGMService;
    private SceneLoader sceneLoader;

    /// <summary>
    /// SW| Grabs the normal color of the player sprite to use for damage flash
    /// </summary>
    void Start()
    {
        _linkGMService = ServiceLocator.Instance.Get<GameManagerService>();
        _playerSprite = this.GetComponentInChildren<SpriteRenderer>();
        _normalColor = _playerSprite.color;
        sceneLoader = GameObject.Find("SceneController").GetComponent<SceneLoader>();
    }

    /// <summary>
    /// When the player collides with something, if it's a bullet, the player takes damage, becomes invincible, and flashes
    /// Triggers the game over state if _playerHealth is <= 0
    ///     *game over state not yet implemented
    ///     
    /// NEW FUNCTIONALITY FOR WALLS TO BE IMPLEMENTED
    /// When player collides with collider tagged "Wall", decrement playerLives in GameManagerService
    /// If not 0, make Splash invincible and push to position 0, 0 (or similar)
    /// If 0, game over state
    /// 
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") == true)
        {
            YouDied();
        }
    }

    void YouDied()
    {
        Debug.Log("YOU DIED");
        gameObject.SetActive(false);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Die");
        sceneLoader.GameOver();
    }

    void PlayerHasBeenHit(Collider2D collision)
    {
        _bInvincible = true;

        Projectile Proj = collision.gameObject.GetComponent<Projectile>();
        if (Proj != null)
            _playerHealth += Proj.GetProjectileValue();

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
    /// SW| Makes the player flash for 2 seconds, the become not invincible
    /// </summary>
    /// <returns></returns>
    IEnumerator EDamageFlash()
    {
        for (int i = 0; i < 10; i++) 
        {
            _playerSprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            yield return new WaitForSeconds(0.1f);
            _playerSprite.color = _normalColor;
            yield return new WaitForSeconds(0.1f);
        }
        _bInvincible = false;
    }
}

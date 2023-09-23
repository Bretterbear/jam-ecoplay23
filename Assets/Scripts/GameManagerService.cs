using Services;
using UnityEngine;


/// <summary>
/// Used for level-wide variable tracking (screen bullet density, player health / energy, etc)
/// </summary>
public class GameManagerService : IService
{
    // --- Private Variable Declarations --- //
    // Environment variables - used for tracking bullet density
    private int liveBulletCount;
    private int peakBulletCount;
    // Player statistics
    private float playerEnergy;
    private int playerLives;

    private float playerMaxEnergy;

    Vector3 playerLocation = Vector3.zero;

    /// <summary>
    /// Called from GameManager
    /// </summary>
    public void Init()
    {
        liveBulletCount = 0;    // Used to store currently live bullets in level (for bullet density)
        peakBulletCount = 0;
    }

    public void recordPlayerLocation(Vector3 playerLoc)
    {
        playerLocation = playerLoc;
    }

    public Vector3 GetPlayerLocation()
    {
        return playerLocation;
    }


    /// <summary>
    /// Used by game manager to get relative bullet density (for FMOD audio tweaks)
    /// </summary>
    /// <returns>a [0,1] float representing bullet density</returns>
    public float getBulletDensity()
    {
        return (((float) liveBulletCount) / ((float) peakBulletCount));
    }

    /// <summary>
    /// Every bullet registers itself on enable
    /// </summary>
    public void RegisterBullet()
    {
        liveBulletCount++;
        if (liveBulletCount > peakBulletCount)
        {
            peakBulletCount = liveBulletCount;
        }
    }

    /// <summary>
    /// Every bullet unregisters itself on disable
    /// </summary>
    public void UnregisterBullet() 
    {
        liveBulletCount--;
    }

    /// <summary>
    /// TMP Function; returns current peak bullet count (to figure out what our level density max is for balancing the audio distortion effect
    /// </summary>
    /// <returns></returns>
    public int GetPeakBulletCount()
    {
        //BH| REMINDER - Remove this func before build & hardcoat peakbulletcount
        return peakBulletCount;
    }


    public float GetPlayerEnergy()
    {
        return playerEnergy;
    }

    public void SetPlayerEnergy(float energyAmount)
    {
        playerEnergy = energyAmount;
    }

    public float GetMaxEnergy()
    {
         
        return playerMaxEnergy;
    }

    public void SetMaxEnergy(float MaxEnergyAmount)
    {
        playerMaxEnergy = MaxEnergyAmount;
        
    }

}
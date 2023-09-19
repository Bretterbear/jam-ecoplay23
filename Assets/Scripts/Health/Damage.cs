using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores damage data. Attach to any object you want to be able to inflict damage
/// As of now damage done by handling on the player in colliders (so tags matter as well
/// </summary>
public class Damage : MonoBehaviour
{
    // --- Public Variable Declarations --- //
    public float damage = 1f;               // 
    public float GetDamage() {  return damage; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static List<GameObject> bulletsInUse;
    // Start is called before the first frame update
    void Start()
    {
        bulletsInUse = new List<GameObject>();
    }

    public static GameObject GetBulletFromPoop()
    {
        for (int i = 0; i < bulletsInUse.Count; i++)
        {
            if (!bulletsInUse[i].activeSelf)
            {
                bulletsInUse[i].GetComponent<Bullet>().ResetTimeToLive();
                bulletsInUse[i].SetActive(true);
                return bulletsInUse[i];
            }
        }
        return null;
    }
}

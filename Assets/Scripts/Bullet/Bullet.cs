using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 velocity;
    public float speed;
    public float rotation;
    public float lifeTime = 5f;
    public float timeToLive;

    // Start is called before the first frame update
    void Start()
    {
        timeToLive = lifeTime;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity * speed * Time.deltaTime);
        timeToLive -= Time.deltaTime;

        if (timeToLive <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetTimeToLive()
    {
        timeToLive = lifeTime;
    }
}
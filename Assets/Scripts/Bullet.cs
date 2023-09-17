using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 velocity;
    public float speed;
    public float rotation;
    public float lifeTime = 2f;
    public float timeToLive;

    public float damage = 1f;

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
    }

    public float GetDamage() { return damage; }
}

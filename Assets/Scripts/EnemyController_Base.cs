using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController_Base : MonoBehaviour
{
    [SerializeField] public Transform[] positionBounds;
    [SerializeField] public float speed = 1f;
    [SerializeField] public float damage = 10f;

    private int patrolTarget;

    // Start is called before the first frame update
    void Start()
    {
        patrolTarget = 0;
    }

    // Update is called once per frame
/// <summary>
/// 
/// </summary>
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, positionBounds[patrolTarget].position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, positionBounds[patrolTarget].position) < 1)
        {
            IncrementPatrolPoint();
        }

    }
    private void IncrementPatrolPoint() //5
    {
        patrolTarget++;
        if (patrolTarget >= positionBounds.Length)
        {
            patrolTarget = 0;
        }
    }

    public float getDamage() {  return damage; }
}
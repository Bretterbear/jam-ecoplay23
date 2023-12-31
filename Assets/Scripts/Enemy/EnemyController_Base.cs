using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController_Base : MonoBehaviour
{
    [SerializeField] public Transform[] positionBounds;
    [SerializeField,Min(1)] public int patrolLength = 10;

    [SerializeField] public float speed = 1f;

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

        if (Vector3.Distance(transform.position, positionBounds[patrolTarget].position) < .025)
        {
            IncrementPatrolPoint();
        }

    }
    private void IncrementPatrolPoint() //5
    {
        patrolTarget++;
        if (patrolTarget >= positionBounds.Length)
        {
            Destroy(this.gameObject);
        }
    }
}
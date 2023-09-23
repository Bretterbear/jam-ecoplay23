using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BH| TODO - Change binary start/stop in layer to a gradual slowdown time permitting
//BH| TODO - Make governance class for root BG object to be able to control all sub layers
//BH| POSS - Add small random differential motion within layer, or the ability to mod speed inside a level


//Crit values: 15,0,0 = bg level start
//            -15,0,0 = bg level end

public class BackgroundScroll : MonoBehaviour
{
    // --- Serialized Variable Declarations --- //
    [Header("Loop Behavior Controls")]
    [Tooltip("How fast content in this layer should move")]
    [SerializeField] private float scrollSpeed = 1f;
    [Tooltip("Controls whether layer elements will scroll on update")]
    [SerializeField] private bool bIsMoving = true;
    [Tooltip("Controls whether layer elements will scroll on update")]
    [SerializeField] private bool bLoopAtFinish = true;

    [Header("Loop Point Designation")]
    [Tooltip("X value of the point where looping BG content should wrap around AT")]
    [SerializeField] private float loopPointLeft = -15f;
    [Tooltip("Right loop pt X value - should be set at your furthest offscreen content to the right")]
    [SerializeField] private float loopPointRight = 13f;

    [Header("Controlled Elements")]
    [Tooltip("References to all game objects you want placed under control in this layer")]
    [SerializeField] GameObject[] objectsUnderControl;

    // --- Non-Serialized Variable Declarations --- //
    private Vector3 moveVector;                     // Vector for scrolling direction

    private void Start()
    {
        moveVector = new Vector3(scrollSpeed * -1, 0f, 0f);
    }

    void Update()
    {
        // Don't update anything is we aren't moving
        if (!bIsMoving)
        {
            return;
        }

        foreach (GameObject obj in objectsUnderControl)
        {
            obj.transform.Translate(moveVector * Time.deltaTime);
            if (bLoopAtFinish && obj.transform.position.x < loopPointLeft)
            {
                obj.transform.position = new Vector3(loopPointRight, obj.transform.position.y);
            }
            else if(!bLoopAtFinish && obj.transform.localPosition.x < loopPointLeft)
            {
                moveVector = moveVector * 0;
            }
        }
    }

    public void StopLayerMovement()
    {
        bIsMoving = false;
    }

    public void StartLayerMovement()
    {
        bIsMoving = true;
    }
}

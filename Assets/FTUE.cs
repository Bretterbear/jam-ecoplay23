using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FTUE : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject ftueOBJ;
    void Start()
    {
        StartCoroutine(FTUETime());

        ftueOBJ = GameObject.Find("FTUETIME2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FTUETime()
    {
        yield return new WaitForSeconds(4);
        ftueOBJ.SetActive(false);
    }



}

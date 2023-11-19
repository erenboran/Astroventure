using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    private void Start()
    {
        Debug.DrawLine(new Vector3(0, 0, 0), new Vector3(10, 0,0),Color.green, 10000f);
    }
}

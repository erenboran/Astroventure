using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{

    private void OnEnable()
    {
        GameManager.Instance.batteryCount++;
        TaskController.Instance.TaskControl3();

    }

    private void OnDisable()
    {
        GameManager.Instance.batteryCount--;
    }
}

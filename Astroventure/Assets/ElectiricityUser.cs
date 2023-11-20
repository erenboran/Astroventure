using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectiricityUser : MonoBehaviour
{
    [SerializeField]
    int usage = 10;

    private void OnEnable()
    {
        GameManager.Instance.electicityUsage += usage;
    }

    private void OnDisable()
    {
        GameManager.Instance.electicityUsage -= usage;
    }
}

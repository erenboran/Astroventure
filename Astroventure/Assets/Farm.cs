using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.farmCount++;
    }

    private void OnDisable() {
        GameManager.Instance.farmCount--;
    }
}

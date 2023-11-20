using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SolarPanel : MonoBehaviour
{
    [SerializeField] Transform[] armsEndPoint;
    [SerializeField] GameObject[] arms;

    private void OnEnable()
    {
        GameManager.Instance.solarPanelCount++;
    }

    private void OnDisable()
    {
        GameManager.Instance.solarPanelCount--;
    }

    private void Start()
    {
        for (int i = 0; i < arms.Length; i++)
        {
            arms[i].transform.DORotateQuaternion(armsEndPoint[i].rotation, 0.75f);
        }

    }


}

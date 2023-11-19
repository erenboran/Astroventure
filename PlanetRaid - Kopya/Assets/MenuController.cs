using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject croshair;

    private void OnEnable()
    {
        GameEvents.Instance.OnBuildMenuOpened += ChangeCorshair;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnBuildMenuOpened -= ChangeCorshair;
    }


    void ChangeCorshair(bool isOpen)
    {
        if(isOpen)
        {
            croshair.SetActive(false);
        }
        else
        {
            croshair.SetActive(true);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResourceElement : MonoBehaviour
{
    [SerializeField]
    Text currentCount,warehouseVolume;
    [SerializeField]
    Image resourceIcon;

    [SerializeField]
    Enums.ResourceTypes resourceType;

    
    void OnEnable()
    {
        GameEvents.Instance.OnResourceChanged+=ChangeResourceText;
    }

    void OnDisable()
    {
        GameEvents.Instance.OnResourceChanged-=ChangeResourceText;
    }

    void ChangeResourceText(Enums.ResourceTypes resourceType, int count)
    {
        if(this.resourceType == resourceType)
        {   int oldCount = 0;
            oldCount = int.Parse(currentCount.text);
           
            currentCount.text=(oldCount+count).ToString();

          
        }

           

    }


}

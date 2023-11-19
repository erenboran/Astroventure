using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PortableMiner : MonoBehaviour
{
    [SerializeField]
    int capacity;
    [SerializeField]
    int currentResource;
    [SerializeField]
    Enums.ResourceTypes resourceType;

    [SerializeField]
    Text capacityText;

    bool _isWorking;

    void OnEnable()
    {
        StartCoroutine(Mine());

    }
    void OnMouseDown()
    {
       UnloadResource();
    }

    void UnloadResource()
    {
        
        GameEvents.Instance.OnResourceChanged?.Invoke(resourceType,currentResource);
       
        currentResource=0;
        capacityText.text = currentResource +"/"+capacity;
        if(!_isWorking)
        {
            StartCoroutine(Mine());

        }
    }

    IEnumerator Mine()
    {
        while(currentResource < capacity)
        {
                        
            _isWorking=true;
            yield return new WaitForSeconds(1);
            currentResource++;
            capacityText.text = currentResource +"/"+capacity;


        }
        _isWorking=false;

    }
}

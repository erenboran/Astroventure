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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mine")
        {


            if (other.GetComponent<MineSource>().resourceType != resourceType)
            {
                resourceType = other.GetComponent<MineSource>().resourceType;
            }

            GameManager.Instance.AddMiner(resourceType);

            TaskController.Instance.TaskControl1(resourceType);
            


        }
    }


}

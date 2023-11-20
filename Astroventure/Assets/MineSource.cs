using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSource : MonoBehaviour, IInteractable
{
    public Enums.ResourceTypes resourceType;
    public int resourceCount;
    public string ShowName()
    {
        return resourceType.ToString()+ " Madeni";
    }
}


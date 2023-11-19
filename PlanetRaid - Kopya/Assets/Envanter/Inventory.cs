using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
   [SerializeField]
   int slotCount;
   [SerializeField]
   GameObject envanterSlot;

   private void Start() 
   {
    for (int i = 0; i < slotCount; i++)
    {
       GameObject newSlot= Instantiate(envanterSlot,transform);
       //newSlot.transform.parent=transform;
    }
   
   
   }



}

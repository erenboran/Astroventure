using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolTip : MonoBehaviour
{
    [SerializeField]
  public  TMP_Text itemName, itemDescription, itemCost;

    Color resource1costColor, resource2costColor;

    public void Init(PlacedObjectTypeSO placedObjectTypeSO)
    {
        itemName.text = placedObjectTypeSO.nameString;

        itemDescription.text = placedObjectTypeSO.description;

        if (placedObjectTypeSO.buildResources.resource1 > GameManager.Instance.buildResources.resource1)
        {
            resource1costColor = Color.red;
        }
        else
        {
            resource1costColor = Color.green;
        }

        if (placedObjectTypeSO.buildResources.resource2 > GameManager.Instance.buildResources.resource2)
        {
            resource2costColor = Color.red;
        }
        else
        {
            resource2costColor = Color.green;
        }


        itemCost.text = "Gerekli Kaynaklar: \n\n" +
                        $"<color=#{ColorUtility.ToHtmlStringRGB(resource1costColor)}>" +
                        "Demir: " + placedObjectTypeSO.buildResources.resource1.ToString() +
                        "</color>" + "\n" +
                        $"<color=#{ColorUtility.ToHtmlStringRGB(resource2costColor)}>" +
                        "Bakır: " + placedObjectTypeSO.buildResources.resource2.ToString() +
                        "</color>";


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildTypes : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   

    [SerializeField]
    string buildName;

   

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameEvents.Instance.OnToolTipActivatedForTypes?.Invoke(buildName, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameEvents.Instance.OnToolTipActivatedForTypes?.Invoke(buildName, false);
    }



    



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image buildImage;

    public PlacedObjectTypeSO buildType;

    public void Init()
    {
        buildImage.sprite = buildType.buildMenuImage;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameEvents.Instance.OnToolTipActivated?.Invoke(buildType, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameEvents.Instance.OnToolTipActivated?.Invoke(buildType, false);
    }



    public void SelectBuildType()
    {
        if (GameEvents.Instance.OnResourceControl.Invoke(buildType.buildResources))
        {
            GameEvents.Instance.OnNewBuildingSelected?.Invoke(buildType);

        }
        else
        {
            GameEvents.Instance.OnOutOfResources?.Invoke();

        }
    }



}

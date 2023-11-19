using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class OutOfResourcesAnim : MonoBehaviour
{
    Vector3 startPosition;

    [SerializeField]
    TMP_Text warningText;

    bool isActive = false;
    private void Awake()
    {
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        GameEvents.Instance.OnOutOfResources += StartAnim;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnOutOfResources -= StartAnim;
    }

    void StartAnim()
    {
        if (isActive)
        {
            return;
        }

        isActive = true;

        warningText.enabled = true;

        transform.DOMoveY(startPosition.y + 20, 0.5f).SetLoops(1, LoopType.Yoyo).OnComplete(() =>
        {
            transform.position = startPosition;
            warningText.enabled = false;
            isActive = false;

        });
    }
}

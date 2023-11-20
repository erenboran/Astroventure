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
        GameEvents.Instance.OnWarningMessage += StartAnim;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnWarningMessage -= StartAnim;
    }

    void StartAnim(string message)
    {
        warningText.text = message;

        if (isActive)
        {
            return;
        }

        isActive = true;

        warningText.enabled = true;

        float messageTimer = 0.5f+message.Length*0.05f;

        transform.DOMoveY(startPosition.y + 50,messageTimer).SetLoops(1, LoopType.Yoyo).OnComplete(() =>
        {
            transform.position = startPosition;
            warningText.enabled = false;
            isActive = false;

        });
    }
}

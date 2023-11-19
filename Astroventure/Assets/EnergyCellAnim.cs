using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class EnergyCellAnim : MonoBehaviour
{

    public GameObject g1, g2, g3;

    void Start()
    {
        g1.transform.DOLocalMoveY(2.6f, 1).SetLoops(-1, LoopType.Yoyo);
        g2.transform.DOLocalMoveY(2.6f, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        g3.transform.DOLocalMoveY(2.6f, 1).SetLoops(-1, LoopType.Yoyo);


    }

    // Update is called once per frame
    void Update()
    {

    }
}

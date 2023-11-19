using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using System;

public class Collectable : MonoBehaviour, ICollectable
{
    [SerializeField]
    Enums.ResourceTypes resourceType;

    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Collider col;

    float moveSpeed = 10;

    // [SerializeField] Vector3 randomOffset;

    private void Start()
    {
        ParabolicMove();
    }

    void ParabolicMove()
    {
        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-2f, 2f), 0, UnityEngine.Random.Range(-2f, 2f));
        rb.AddForce(randomOffset + Vector3.up * 5, ForceMode.Impulse);

    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(rb);
            col.isTrigger = true;
        }



    }
    // ONMOUSEENTER
    void OnMouseEnter()

    {
        Collect(PlayerController.Instance.collectPoint);
    }






    public void Collect(Transform othertransform)
    {
        StartCoroutine(MoveToTarget(othertransform));
        //   transform.DOScale(Vector3.zero, 0.5f);
    }

    private IEnumerator MoveToTarget(Transform target)
    {
        float time = 0;


        while (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            time += Time.deltaTime;
            moveSpeed += time * 2;
            transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}

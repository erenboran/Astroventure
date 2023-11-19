using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    private Vector3 shootDir;

    [SerializeField]
    private float moveSpeed;

    public int damage;

    public void Setup(Vector3 shootDir,Transform enemy)
    {
        this.shootDir = shootDir;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
        transform.LookAt(enemy);
    }

    float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
       
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }




    private void Update()
    {
        transform.position += shootDir * moveSpeed * Time.deltaTime;
       
    }

    private void OnTriggerEnter(Collider other)
    {
        




        Destroy(gameObject);

    }

}

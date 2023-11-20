using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{

    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 50f;
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<IEnemy>() is IEnemy enemy && enemy != null)
        {
            enemy.TakeDamage(25);

            Instantiate(vfxHitRed, transform.position, Quaternion.identity);
        }
        else
        {
          
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}
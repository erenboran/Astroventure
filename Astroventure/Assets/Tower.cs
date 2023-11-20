using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Tower : MonoBehaviour
{
    public List<EnemyAIManager> enemies = new List<EnemyAIManager>();

    [SerializeField] float fireRate;

    EnemyAIManager target;

    [SerializeField] Transform spawnBulletPositionRight, spawnBulletPositionLeft;
    [SerializeField] Transform bulletProjectile;
    [SerializeField] Transform towerHead;
    [SerializeField] GameObject rightArm, leftArm;
    [SerializeField] Transform rightStartPoint, rightEndPoint, leftStartPoint, leftEndPoint; 
    [SerializeField] int fireCost;
    [SerializeField] AudioSource audioSource;

    [SerializeField]  bool isFiring;

    Quaternion startRotation;

    private void Start()
    {
        startRotation = towerHead.transform.rotation;
    }

    private void Update()
    {
        if (target== null ||  target.health <= 0)
        {
            SelectTarget();

            StartCoroutine(LookAtStartRotation());

        }

        else
        {
            towerHead.transform.LookAt(target.gameObject.transform);

            Fire();
        }

    }

    IEnumerator LookAtStartRotation()
    {
        yield return new WaitForSeconds(1);
        if (target== null)
        {
            towerHead.transform.DORotateQuaternion(startRotation, 0.5f);

        }

    }

    void Fire() 
    {
        if (!isFiring)
        {
            isFiring = true;

            StartCoroutine(FireRuotine());
        }

    
    }

    IEnumerator FireRuotine()
    {
        yield return new WaitForSeconds(fireRate);

      
        if (target!=null && GameManager.Instance.currentBattery >= fireCost && target.health > 0)
        {
            Vector3 aimDir = (target.GetComponent<EnemyAIManager>().hitPoint.position - spawnBulletPositionRight.position).normalized;

            Instantiate(bulletProjectile, spawnBulletPositionRight.position, Quaternion.LookRotation(aimDir, Vector3.up));

            GameManager.Instance.currentBattery -=fireCost;

            audioSource.pitch = Random.Range(0.8f, 1);
            audioSource.Play();

            rightArm.transform.DOMove(rightStartPoint.position, 0.1f).OnComplete(() =>
            {
                rightArm.transform.DOMove(rightEndPoint.position, 0.1f).OnComplete(()=> 
                
                {
                    if (target != null && GameManager.Instance.currentBattery >= fireCost && target.health>0)
                    {
                        Vector3 aimDir = (target.GetComponent<EnemyAIManager>().hitPoint.position - spawnBulletPositionLeft.position).normalized;

                        Instantiate(bulletProjectile, spawnBulletPositionLeft.position, Quaternion.LookRotation(aimDir, Vector3.up));

                       GameManager.Instance.currentBattery -= fireCost;

                        audioSource.pitch = Random.Range(0.8f, 1);
                        audioSource.Play();

                        leftArm.transform.DOMove(leftStartPoint.position, 0.1f).OnComplete(() =>
                        {
                            leftArm.transform.DOMove(leftEndPoint.position, 0.1f).OnComplete(() =>
                            {

                                isFiring = false;

                            });


                        });
                    }

                    else
                    {
                        isFiring = false;
                    }
                    
                   


                    });
            }
            );



        }
        else
        {
            isFiring = false;
        }

       

    }





   public void SelectTarget()
    {
        if (enemies.Count>0)
        {
            if (enemies[0] !=null)
            {
                target = enemies[0];
            }
            else
            {
                enemies.RemoveAt(0);
                target = null;
            }

            
        }

        else
        {
            target = null;
        }
    }



   
}

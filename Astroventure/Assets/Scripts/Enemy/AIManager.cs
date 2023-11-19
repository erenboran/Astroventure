using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIManager : MonoBehaviour
{

    public enum PositionState {State,Defance,Attack }

    public Transform hitPoint;

    public Transform _targetEnemy;

    public NavMeshAgent agent;

    public Animator animator;

    public float fireRate;

    public float range;

    public int damage;

    [SerializeField]
    Transform firePoint;
   
    [SerializeField]
    GameObject bullet;

    public List<GameObject> enemiesInRange;

    public float health=100;

   public bool isActiceFire;



    public void GotoTarget(Transform point)
    {

        agent.SetDestination(point.position);

        agent.isStopped = false;

        animator.SetBool("isRun", true);

    }
    public bool CheckArrive(float range)
    {
        if (agent.hasPath && agent.remainingDistance < range)
        {
            animator.SetBool("isRun", false);

            agent.isStopped = true;

            return true;
        }

        return false;
    }

    public IEnumerator Fire(float _fireRate )
    {
        isActiceFire = true;

        yield return new WaitForSeconds(_fireRate);
        
       
        if (enemiesInRange.Count > 0)
        {
            if (enemiesInRange[0] != null)
            {
                GameObject newBullet = Instantiate(bullet, firePoint.position, Quaternion.identity);

                Vector3 shootDir = (enemiesInRange[0].transform.position - firePoint.position).normalized;

                newBullet.GetComponent<Bullet>().Setup(shootDir, enemiesInRange[0].GetComponentInParent<AIManager>().hitPoint);

                newBullet.GetComponent<Bullet>().damage = damage;

                StartCoroutine(Fire(_fireRate));

            }

            else
            {
                enemiesInRange.RemoveAt(0);
                
                isActiceFire = false;
            }
         
        }

        else
        {
            isActiceFire = false;
        }
    }




    public void CheckArrive()
    {
       
        if (agent.hasPath && agent.remainingDistance <0.2f)
        {
            animator.SetBool("isRun", false);

            agent.isStopped = true;
        }
    }

  
}

        



  




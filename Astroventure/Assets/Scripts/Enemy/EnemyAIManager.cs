using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using DG.Tweening;


public class EnemyAIManager : MonoBehaviour, IEnemy
{
    [SerializeField]
    int attackDamage;
    public List<Transform> targetPoints;

    public Transform _targetEnemy;

    public enum PositionState { State, Defance, Attack }

    public Transform hitPoint;

    public NavMeshAgent agent;

    public Animator animator;

    public Transform playerBase;

    public float health = 100;

    bool targetIsBase = false;

    [SerializeField]
    GameObject[] enemyModel;

    [SerializeField] Transform targetInRange;






    public IEnumerator SetupSpawn(float time, Transform target)
    {
        int randomModel = Random.Range(0, enemyModel.Length);

        enemyModel[randomModel].SetActive(true);

        yield return new WaitForSeconds(time);

        playerBase = target;

        targetIsBase = true;

        GotoTarget(target);

    }


    public void GotoTarget(Transform point)
    {
        agent.SetDestination(point.position);

        agent.isStopped = false;

        animator.SetBool("isRun", true);

        StartCoroutine(CheckArriveRoutine());

    }

    public bool CheckArrive(float range)
    {
        if (agent.hasPath && agent.remainingDistance < range)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public IEnumerator CheckArriveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (CheckArrive(2f) || targetInRange != null)
            {
                animator.SetBool("isRun", false);

                animator.SetBool("isAttack", true);

                agent.isStopped = true;

                break;

            }

            else if (targetIsBase && targetPoints.Count > 0)
            {

                FindClosestEnemy();

                break;

            }


        }

    }




    public void Attack()
    {
        if (_targetEnemy != null)
        {
            if (CheckArrive(0.2f))
            {
                _targetEnemy.GetComponent<IDamageable>().TakeDamage(attackDamage);
            }

            else if (targetInRange != null)
            {
                targetInRange.GetComponent<IDamageable>().TakeDamage(attackDamage);
            }
            else
            {
                FindClosestEnemy();
            }


        }
        else
        {
            FindClosestEnemy();
        }
    }


    void FindClosestEnemy()
    {


        foreach (Transform enemy in targetPoints)
        {
            if (enemy == null)
            {
                targetPoints.Remove(enemy);

                GotoTarget(playerBase);

                break;
            }

            if (_targetEnemy == null || Vector3.Distance(transform.position, enemy.position) < Vector3.Distance(transform.position, _targetEnemy.position))
            {
                _targetEnemy = enemy;
            }
        }

        if (_targetEnemy == null || Vector3.Distance(transform.position, playerBase.position) < Vector3.Distance(transform.position, _targetEnemy.position))
        {
            GotoTarget(playerBase);
            targetIsBase = true;
        }

        else 
        {
            GotoTarget(_targetEnemy);
            targetIsBase = false;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            targetInRange = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            targetInRange = null;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        transform.DOShakePosition(0.25f, 0.5f, 10, 90f, false, true);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        animator.SetBool("isDie", true);

        animator.SetBool("isAttack", false);
        agent.isStopped = true;

        agent.enabled = false;

        Destroy(gameObject, 2.5f);
    }

}

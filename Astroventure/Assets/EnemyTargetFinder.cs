using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetFinder : MonoBehaviour
{
    [SerializeField] EnemyAIManager enemyAIManager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            enemyAIManager.targetPoints.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            enemyAIManager.targetPoints.Remove(other.transform);
        }
    }
}

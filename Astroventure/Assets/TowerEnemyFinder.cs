using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEnemyFinder : MonoBehaviour
{
    Tower tower;

    private void Start()
    {
        tower = GetComponentInParent<Tower>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyAIManager>() != null)
        {
            tower.enemies.Add(other.GetComponent<EnemyAIManager>());

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EnemyAIManager>() != null)
        {
            tower.enemies.Remove(other.GetComponent<EnemyAIManager>());

            tower.SelectTarget();
        }
    }
}

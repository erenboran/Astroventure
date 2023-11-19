using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemySO enemySO;

    private EnemySO enemy;

    private NavMeshAgent _agent;

    public Transform _targetToFollow;

    private void Awake()
    {

        _agent = GetComponent<NavMeshAgent>();

        enemy = enemySO.ShallowCopy();


    }





}


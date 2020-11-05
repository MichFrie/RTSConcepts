using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{

    [Header("Components")]
    public GameObject selectionVisual;
    NavMeshAgent navAgent;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void ToggleSelectionVisual(bool selected)
    {
        selectionVisual.SetActive(selected);
    }

    public void MoveToPosition(Vector3 pos)
    {
        //navAgent.isStopped = false;
        navAgent.SetDestination(pos);
    }
}

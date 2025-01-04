using UnityEngine;
using UnityEngine.AI;

public class MeleeMonster : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent navMeshAgent;

    bool isTargetFind;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(!isTargetFind)
        {
            if(Mathf.Abs((player.transform.position - transform.position).magnitude) < 30f)
                isTargetFind = true;
        }
        else
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
    }
}
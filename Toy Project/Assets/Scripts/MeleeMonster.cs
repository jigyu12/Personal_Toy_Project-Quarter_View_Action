 using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMonster : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent navMeshAgent;
    private readonly int hashWalk = Animator.StringToHash("Walk");
    private readonly int hashDead = Animator.StringToHash("IsDead");
    private Animator animator;

    bool isTargetFind;
    
    private int hp = 7;
    
    public GameManager gm;

    public bool IsDead
    {
        get;
        private set;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (IsDead)
            return;
        
        if(!isTargetFind)
        {
            if (Mathf.Abs((player.transform.position - transform.position).magnitude) < 30f)
            {
                isTargetFind = true;
                
                animator.SetTrigger(hashWalk);
            }
        }
        else
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
    }
    
    public void Damaged(int damage)
    {
        if (IsDead)
            return;
        
        hp -= damage;

        if (hp <= 0)
        {
            IsDead = true;
            
            navMeshAgent.isStopped = true;
            
            StartCoroutine(Dead());
        }
    }

    IEnumerator Dead()
    {
        animator.SetTrigger(hashDead);
        
        yield return new WaitForSeconds(1f);
        
        gm.SubMonsterCount();
        Destroy(gameObject);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MeleeWeapon"))
        {
            Damaged(3);
        }
    }
}
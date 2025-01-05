using System.Collections;
using UnityEngine;

public class RangeMonster : MonoBehaviour
{
    private GameObject player;
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashDead = Animator.StringToHash("IsDead");
    private Animator animator;
    
    bool isTargetFind;
    
    bool isAttacking;
    
    public MonsterMissile missile;

    private int hp = 5;
    
    public GameManager gm;
    
    public bool IsDead
    {
        get;
        private set;
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
            }
        }
        else
        {
            transform.LookAt(player.transform);
            
            if (!isAttacking)
            {
                  StartCoroutine( Attack());
            }
            
            if (Mathf.Abs((player.transform.position - transform.position).magnitude) >= 30f)
            {
                isTargetFind = false;
                
                animator.SetBool(hashAttack, false);
            }
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        animator.SetBool(hashAttack, true);

        yield return new WaitForSeconds(0.55f);

        MonsterMissile m = Instantiate(missile);
        m.Initialize(transform.position + transform.forward * 5f + transform.up * 2f, transform.forward);

        yield return new WaitForSeconds(0.45f);

        animator.SetBool(hashAttack, false);

        yield return new WaitForSeconds(1f);

        isAttacking = false;
    }


    public void Damaged(int damage)
    {
        if (IsDead)
            return;
        
        hp -= damage;

        if (hp <= 0)
        {
            IsDead = true;
            
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

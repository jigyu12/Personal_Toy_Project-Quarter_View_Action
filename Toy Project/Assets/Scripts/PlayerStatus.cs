using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private int hp = 5;

    private bool isDamaged;
    
    private Animator animator;
    private readonly int hashDead = Animator.StringToHash("IsDead");

    public bool IsDead
    {
        get; private set;
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    
    void OnCollisionStay(Collision collision)
    {
        if (isDamaged || IsDead)
            return;
        
        if (collision.transform.CompareTag("MeleeMonster")||
            collision.transform.CompareTag("MonsterMissile"))
        {
            hp -= 1;
            
            Debug.Log(hp);

            if (hp <= 0)
            {
                animator.SetTrigger(hashDead);
                
                IsDead = true;
            }

            StartCoroutine(Damaged());
        }
    }

    public void SubHp()
    {
        if (isDamaged || IsDead)
            return;
        
        hp -= 1;
            
        Debug.Log(hp);
        
        if (hp <= 0)
        {
            animator.SetTrigger(hashDead);
                
            IsDead = true;
        }

        StartCoroutine(Damaged());
    }

    public IEnumerator Damaged()
    {
        isDamaged = true;
        
        yield return new WaitForSeconds(1f);
        
        isDamaged = false;
    }
}

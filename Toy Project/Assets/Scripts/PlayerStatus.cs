using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private int hp;

    private bool isDamaged;
    
    private Animator animator;
    private readonly int hashDead = Animator.StringToHash("IsDead");

    public Transform hpUI;
    
    public GameManager gameManager;

    public bool IsDead
    {
        get; private set;
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        
        hp = 5;
        
        IsDead = false;
    }
    
    void OnCollisionStay(Collision collision)
    {
        if (isDamaged || IsDead)
            return;
        
        if (collision.transform.CompareTag("MeleeMonster")||
            collision.transform.CompareTag("MonsterMissile"))
        {
            hp -= 1;
            hpUI.GetComponent<HpUI>().ChangeHp(hp);
            
            Debug.Log(hp);

            if (hp <= 0)
            {
                animator.SetTrigger(hashDead);
                
                IsDead = true;
                
                gameManager.PlayerDead();
            }

            StartCoroutine(Damaged());
        }
    }

    public void SubHp()
    {
        if (isDamaged || IsDead)
            return;
        
        hp -= 1;
        hpUI.GetComponent<HpUI>().ChangeHp(hp);
            
        Debug.Log(hp);
        
        if (hp <= 0)
        {
            animator.SetTrigger(hashDead);
                
            IsDead = true;
            
            gameManager.PlayerDead();
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

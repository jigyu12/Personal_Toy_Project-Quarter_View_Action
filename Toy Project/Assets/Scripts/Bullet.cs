using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerMove playerMove;

    private Rigidbody rb;

    private float bulletSpeed;

    private Vector3 dir;
    private Quaternion moveRotation;

    private GameObject firePos;
    
    private bool isDead;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        firePos = GameObject.FindGameObjectWithTag("FirePos");
    }

    void Start()
    {
        bulletSpeed = 40f;
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + dir.normalized * (bulletSpeed * Time.deltaTime));
    }

    public IEnumerator Fire()
    {
        dir = playerMove.GetPlayerMoveDirection();
        moveRotation = Quaternion.LookRotation(dir);
        rb.MoveRotation(moveRotation);
        transform.position = firePos.transform.position;

        yield return new WaitForSeconds(3f);
        
        if(!isDead)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("MeleeMonster"))
            other.GetComponent<MeleeMonster>().Damaged(2);
        else if (other.transform.CompareTag("RangeMonster"))
            other.GetComponent<RangeMonster>().Damaged(2);

        isDead = true;
        Destroy(gameObject);
    }
}
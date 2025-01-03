using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour
{
    private PlayerMove playerMove;

    private Rigidbody rb;

    private float bulletSpeed;

    private Vector3 dir;
    private Quaternion moveRotation;

    private GameObject firePos;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        firePos = GameObject.FindGameObjectWithTag("FirePos");
    }

    void Start()
    {
        bulletSpeed = 20f;
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + dir.normalized * bulletSpeed * Time.deltaTime);
    }

    public IEnumerator Fire()
    {
        dir = playerMove.GetPlayerMoveDirection();
        moveRotation = Quaternion.LookRotation(dir);
        rb.MoveRotation(moveRotation);
        transform.position = firePos.transform.position;

        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
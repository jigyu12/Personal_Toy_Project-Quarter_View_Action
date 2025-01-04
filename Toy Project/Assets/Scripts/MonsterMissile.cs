using System.Collections;
using UnityEngine;

public class MonsterMissile : MonoBehaviour
{
    private Rigidbody rb;

    private float bulletSpeed;

    private Vector3 dir;
    private Quaternion moveRotation;
    
    private bool isDead;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        bulletSpeed = 20f;
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + dir.normalized * (bulletSpeed * Time.deltaTime));
    }
    
    public void Initialize(Vector3 position, Vector3 direction)
    {
        dir = direction;

        moveRotation = Quaternion.LookRotation(direction);
        Quaternion extraRotation = Quaternion.Euler(0, -90, 0);
        moveRotation *= extraRotation;

        rb.MoveRotation(moveRotation);
        transform.position = position;

        StartCoroutine(SelfDestruct()); // 미사일 수명을 독립적으로 관리
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(3f);

        if (!isDead)
            Destroy(gameObject);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.GetComponent<PlayerStatus>().SubHp();
            
            isDead = true;
            Destroy(gameObject);
        }
    }
}

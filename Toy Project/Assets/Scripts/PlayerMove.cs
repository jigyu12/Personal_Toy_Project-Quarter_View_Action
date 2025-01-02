using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Animator animator;
    private readonly int hashMove = Animator.StringToHash("Move");

    private float hMove;
    private float vMove;
    private float speed;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        speed = 10f;
    }

    void Update()
    {
        hMove = Input.GetAxisRaw("Horizontal");
        vMove = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(-hMove, 0, -vMove);

        rigidbody.MovePosition(transform.position + moveDirection.normalized * speed * Time.deltaTime);
        
        if(moveDirection != Vector3.zero)
        {
            Quaternion moveRotation = Quaternion.LookRotation(moveDirection);

            rigidbody.MoveRotation(moveRotation);
        }

        animator.SetFloat(hashMove, moveDirection.magnitude);
    }
}
using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rigidbody;

    private Animator animator;

    private PlayerAttack playerAttack;

    Vector3 moveDirection;

    private readonly int hashMove = Animator.StringToHash("Move");
    private readonly int hashJump = Animator.StringToHash("IsJumping");
    private readonly int hashDodge = Animator.StringToHash("IsDodge");

    private float hMove;
    private float vMove;
    private float originalSpeed = 20f;
    private float speed;
    private float jumpPower;

    public bool IsJumping
    {
        get; private set;
    }

    public bool IsDodge
    { 
        get; private set;
    }

    private string hMoveString = "Horizontal";
    private string vMoveString = "Vertical";
    private string jumpString = "Jump";
    private string dodgeString = "Fire3";
    private string groundTagString = "Ground";

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Start()
    {
        speed = originalSpeed;
        jumpPower = 8f;

        IsJumping = false;
        IsDodge = false;
    }

    void Update()
    {
        hMove = Input.GetAxisRaw(hMoveString);
        vMove = Input.GetAxisRaw(vMoveString);

        if(!IsDodge && !IsJumping && Input.GetButtonDown(jumpString))
            Jump();

        if (!IsDodge && !IsJumping && Input.GetButtonDown(dodgeString) && moveDirection != Vector3.zero)
            StartCoroutine(Dodge());
    }

    void FixedUpdate()
    {
        if (!IsJumping && !IsDodge)
            moveDirection = new Vector3(-hMove, 0, -vMove);

        if (playerAttack.IsAttack)
            moveDirection = Vector3.zero;

        if (moveDirection != Vector3.zero)
        {
            Quaternion moveRotation = Quaternion.LookRotation(moveDirection);
            rigidbody.MoveRotation(moveRotation);

            RaycastHit hit;
            if (!Physics.Raycast(transform.position, moveDirection, out hit, 3.5f))
            {
                rigidbody.MovePosition(transform.position + moveDirection.normalized * speed * Time.deltaTime);
            }
        }

        animator.SetFloat(hashMove, moveDirection.magnitude);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag(groundTagString))
        {
            IsJumping = false;

            animator.SetBool(hashJump, false);
        }
    }

    void Jump()
    {
        IsJumping = true;

        rigidbody.AddForce(transform.up * jumpPower, ForceMode.Impulse);

        animator.SetBool(hashJump, true);
    }

    IEnumerator Dodge()
    {
        IsDodge = true;
        speed = originalSpeed * 1.7f;
        animator.SetBool(hashDodge, true);

        yield return new WaitForSeconds(0.75f);

        IsDodge = false;
        speed = originalSpeed;
        animator.SetBool(hashDodge, false);
    }

    public Vector3 GetPlayerMoveDirection()
    {
        return transform.forward;
    }
}
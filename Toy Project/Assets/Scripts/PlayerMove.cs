using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rigidbody;

    private Animator animator;

    Vector3 moveDirection;

    private readonly int hashMove = Animator.StringToHash("Move");
    private readonly int hashJump = Animator.StringToHash("IsJumping");
    private readonly int hashDodge = Animator.StringToHash("IsDodge");

    private float hMove;
    private float vMove;
    private float originalSpeed = 20f;
    private float speed;
    private float jumpPower;

    private bool isJumping;
    private bool isDodge;

    private string hMoveString = "Horizontal";
    private string vMoveString = "Vertical";
    private string jumpString = "Jump";
    private string dodgeString = "Fire3";
    private string groundTagString = "Ground";

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        speed = originalSpeed;
        jumpPower = 8f;

        isJumping = false;
        isDodge = false;
    }

    void Update()
    {
        hMove = Input.GetAxisRaw(hMoveString);
        vMove = Input.GetAxisRaw(vMoveString);

        if(!isDodge && !isJumping && Input.GetButtonDown(jumpString))
            Jump();

        if (!isDodge && !isJumping && Input.GetButtonDown(dodgeString) && moveDirection != Vector3.zero)
            StartCoroutine(Dodge());
    }

    void FixedUpdate()
    {
        if (!isJumping && !isDodge)
            moveDirection = new Vector3(-hMove, 0, -vMove);

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
            isJumping = false;

            animator.SetBool(hashJump, false);
        }
    }

    void Jump()
    {
        isJumping = true;

        rigidbody.AddForce(transform.up * jumpPower, ForceMode.Impulse);

        animator.SetBool(hashJump, true);
    }

    IEnumerator Dodge()
    {
        isDodge = true;
        speed = originalSpeed * 1.7f;
        animator.SetBool(hashDodge, true);

        yield return new WaitForSeconds(0.75f);

        isDodge = false;
        speed = originalSpeed;
        animator.SetBool(hashDodge, false);
    }
}
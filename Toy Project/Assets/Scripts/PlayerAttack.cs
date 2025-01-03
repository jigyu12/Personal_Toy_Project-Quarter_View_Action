using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour
{
    enum WeaponType
    {
        Melee,
        Range
    }

    private readonly int hashMeleeAttack = Animator.StringToHash("MeleeAttack");
    private readonly int hashRangeAttack = Animator.StringToHash("RangeAttack");

    private Animator animator;

    private PlayerMove playerMove;

    private WeaponType weaponType = WeaponType.Melee;

    private GameObject meleeWeapon;
    private BoxCollider meleeCollider;

    private GameObject rangeWeapon;

    public Bullet playerBullet;

    public bool IsAttack
    {
        get; 
        private set;
    } = false;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerMove = GetComponent<PlayerMove>();
    }

    void Start()
    {
        meleeWeapon = GameObject.FindGameObjectWithTag("MeleeWeapon");
        meleeCollider = meleeWeapon.GetComponent<BoxCollider>();
        meleeCollider.enabled = false;

        rangeWeapon = GameObject.FindGameObjectWithTag("RangeWeapon");
        rangeWeapon.SetActive(false);
    }

    void Update()
    {
        SwitchWeapon();

        if (!IsAttack && Input.GetMouseButton((int)MouseButton.Left) && !playerMove.IsDodge && !playerMove.IsJumping)
        {
            if(weaponType == WeaponType.Melee)
                StartCoroutine(MeleeAttack());
            else if(weaponType == WeaponType.Range)
                StartCoroutine(RangeAttack());
        }
    }

    IEnumerator MeleeAttack()
    {
        IsAttack = true;
        animator.SetTrigger(hashMeleeAttack);
        meleeCollider.enabled = true;

        yield return new WaitForSeconds(0.7f);

        IsAttack = false;
        meleeCollider.enabled = false;
    }

    IEnumerator RangeAttack()
    {
        IsAttack = true;
        animator.SetTrigger(hashRangeAttack);
        Fire();

        yield return new WaitForSeconds(0.4f);

        IsAttack = false;
    }

    void SwitchWeapon()
    {
        if (IsAttack)
            return;

        if(Input.GetKeyDown(KeyCode.Alpha1) && weaponType == WeaponType.Range)
        {
            weaponType = WeaponType.Melee;

            meleeWeapon.SetActive(true);
            rangeWeapon.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && weaponType == WeaponType.Melee)
        {
            weaponType = WeaponType.Range;

            meleeWeapon.SetActive(false);
            rangeWeapon.SetActive(true);
        }
    }

    void Fire()
    {
        Bullet bullet = Instantiate(playerBullet);

        StartCoroutine(bullet.Fire());
    }
}
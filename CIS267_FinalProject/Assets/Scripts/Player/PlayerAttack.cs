using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Rigidbody2D rb2d;
    public Animator animator;
    public AudioSource slash;
    public bool attacking = false;
    private float attackTime;
    private float animationLength;
    public float attackCooldown;
    public GameObject inventory;

    // Start is called before the first frame update
    void Start()
    {
        animationLength = 0.5f;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !attacking && !inventory.activeInHierarchy && FindObjectOfType<WeaponHolster>().hasWeapon)
        {
            animator.SetTrigger("Attacking");
            animator.SetBool("Attack", false);
            attacking = true;
            FindObjectOfType<AudioManager>().Play("Slash");
        }

        if (attacking)
        {
            attackTime += Time.deltaTime;
            rb2d.velocity = Vector2.zero;

            if (attackTime >= animationLength)
            {
                attacking = false;
            }

            if (attackTime >= attackCooldown)
            {
                attackTime = 0;
                attacking = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public int itemInHolster;
    Rigidbody2D rb2d;
    public Animator animator;
    Vector3 moveDelta;
    public int speed;
    float cooldown = 0f;
    float time = 0;
    bool isDashing = false;
    public float dodgeForce;
    float dashTime = 0.22f;
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //inventory = GameObject.Find("Inventory");
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= cooldown)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isDashing = true;
                cooldown = 3f;
                time = 0f;
            }
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
    }
    private void FixedUpdate()
    {
        if (!gameObject.GetComponent<PlayerAttack>().attacking && !InventoryUI.isActive)
        {
            // Press shift to Dodge
            if (isDashing)
            {
                time += Time.deltaTime;
                if (time <= dashTime)
                {
                    rb2d.velocity = new Vector2(animator.GetFloat("lastMoveHorizontal"), animator.GetFloat("lastMoveVertical")) * dodgeForce * Time.deltaTime;
                }

                else
                {
                    isDashing = false;
                    time = 0;
                }
            }

            else
            {
                Move();
            }

            animator.SetFloat("Vertical", moveDelta.y);
            animator.SetFloat("Horizontal", moveDelta.x);
        }

        else
        {
            rb2d.velocity = new Vector2(0f, 0f);
            animator.SetFloat("Vertical", 0f);
            animator.SetFloat("Horizontal", 0f);
        }
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Move player at normal speed
        rb2d.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed * Time.fixedDeltaTime;
        animator.speed = 1f;

        // Reset MoveDelta
        moveDelta = new Vector3(x, y, 0);

        // /* ANIMATION STUFF

        if (Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1 || Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1)
        {
            animator.SetFloat("lastMoveHorizontal", moveDelta.x);
            animator.SetFloat("lastMoveVertical", moveDelta.y);
        }
    }

    public void UseItem(Item item)
    {
        WeaponHolster weaponHolster = FindObjectOfType<WeaponHolster>();
        if (item.GetType() == typeof(ScriptableWeapon))
        {
            weaponHolster.scriptableWeapon = item as ScriptableWeapon;
            weaponHolster.GetComponent<SpriteRenderer>().sprite = item.sprite;
            weaponHolster.hasWeapon = true;
            if (!GameObject.Find("Inventory"))
            {
                //if (weaponHolster.SelectedItemIcon) weaponHolster.SelectedItemIcon.enabled = true;

            }
        }
        else if (item.GetType() == typeof(Food))
        {
            weaponHolster.ResetWeapon();
            Food food = item as Food;
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            playerHealth.addHealth(food.healthValue);
            PlayerScore playerScore = GetComponent<PlayerScore>();
            playerScore.addScore(food.xpValue);
            Hotbar.instance.Remove(item);
        }
        else
        {
            weaponHolster.ResetWeapon();
        }
    }

    private void ResetColor()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color32(255, 255, 255, 255);
    }

    public void Flash()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color32(255, 100, 100, 255);
        Invoke(nameof(ResetColor), 0.2f);
    }

    public void SetHorziontalDirection(int x)
    {
        animator.SetFloat("lastMoveHorizontal", x);
    }

    public void SetVerticalDirection(int y)
    {
        animator.SetFloat("lastMoveVertical", y);
    }
}

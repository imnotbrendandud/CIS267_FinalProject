using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    Vector2[] colliderPoints;
    Transform t;
    PolygonCollider2D polygonCollider2D;
    private Rigidbody2D rb2d;
    private Animator animator;
    private Transform target;
    [Header("Speed")]
    [Range(1f, 20f)]
    [SerializeField] private float speed;
    [Header("Follow Range")]
    [SerializeField] private float minRange;
    [SerializeField] private float maxRange;
    [Header("Attacking")]
    [Range(0f, 5f)]
    [SerializeField] private float attackRate;
    float nextWaypointDistance = 1f;
    private float time;
    private bool hasFollowed = false;
    private bool attackedOnce;
    Path path;
    Seeker seeker;
    LayerMask blockingLayer;
    int currentWaypoint = 0;
    bool hitEndOfRoamingPath = false;
    bool reachedEndOfPath = false;
    public int offset;

    // Start is called before the first frame update
    void Start()
    {
        t = gameObject.GetComponent<Transform>();
        time = attackRate;
        hasFollowed = false;
        attackedOnce = false;
        target = FindObjectOfType<Player>().transform;
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
        polygonCollider2D = gameObject.GetComponent<PolygonCollider2D>();
        blockingLayer = LayerMask.NameToLayer("Blocking");

        colliderPoints = polygonCollider2D.points;
        polygonCollider2D.offset = new Vector2(-t.parent.position.x, -t.parent.position.y);

        colliderPoints[0] = new Vector2(t.position.x + offset, t.position.y - offset);
        colliderPoints[1] = new Vector2(t.position.x + offset, t.position.y);
        colliderPoints[2] = new Vector2(t.position.x + offset, t.position.y + offset);
        colliderPoints[3] = new Vector2(t.position.x, t.position.y + offset);
        colliderPoints[4] = new Vector2(t.position.x - offset, t.position.y + offset);
        colliderPoints[5] = new Vector2(t.position.x - offset, t.position.y);
        colliderPoints[6] = new Vector2(t.position.x - offset, t.position.y - offset);
        colliderPoints[7] = new Vector2(t.position.x, t.position.y - offset);

        polygonCollider2D.points = colliderPoints;

        InvokeRepeating("UpdatePath", 0f, .5f);
        InvokeRepeating("UpdateRoamingPath", 0f, 5f);
    }

    void UpdatePath()
    {
        float distanceX = Mathf.Abs((Mathf.Abs(target.position.x) - Mathf.Abs(rb2d.position.x)));
        float distanceY = Mathf.Abs((Mathf.Abs(target.position.y) - Mathf.Abs(rb2d.position.y)));
        RaycastHit2D raycastUp = Physics2D.Raycast(rb2d.position, Vector2.up, 5f);
        RaycastHit2D raycastDown = Physics2D.Raycast(rb2d.position, -Vector2.up, 5f);
        RaycastHit2D raycastRight = Physics2D.Raycast(rb2d.position, Vector2.right, 5f);
        RaycastHit2D raycastLeft = Physics2D.Raycast(rb2d.position, -Vector2.right, 5f);
        if (seeker.IsDone() && Vector3.Distance(target.position, transform.position) <= maxRange && (hasFollowed || hitEndOfRoamingPath) && (raycastUp.transform.gameObject.layer == blockingLayer || raycastDown.transform.gameObject.layer == blockingLayer || raycastRight.transform.gameObject.layer == blockingLayer || raycastLeft.transform.gameObject.layer == blockingLayer))
        {
            seeker.StartPath(rb2d.position, transform.parent.position, OnPathComplete);
        }

        else if (seeker.IsDone() && Vector3.Distance(target.position, transform.position) <= maxRange && (hasFollowed || hitEndOfRoamingPath) && (raycastUp.transform.gameObject.layer != blockingLayer || raycastDown.transform.gameObject.layer != blockingLayer || raycastRight.transform.gameObject.layer != blockingLayer || raycastLeft.transform.gameObject.layer != blockingLayer))
        {
            seeker.StartPath(rb2d.position, target.position, OnPathComplete);
        }

        else if (seeker.IsDone() && Vector3.Distance(target.position, transform.position) > maxRange && hasFollowed)
        {
            seeker.StartPath(rb2d.position, transform.parent.position, OnPathComplete);
        }
    }

    void UpdateRoamingPath()
    {
        if (Vector3.Distance(target.position, transform.position) > maxRange && !hasFollowed)
        {
            int rand;
            rand = Random.Range(0, 8);

            seeker.StartPath(rb2d.position, polygonCollider2D.points[rand], OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }

        else
        {
            reachedEndOfPath = false;
        }

        if (Vector3.Distance(target.position, transform.position) <= maxRange)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2d.position).normalized;

            Vector2 force = direction * speed * Time.deltaTime * 100;

            float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWaypoint]);

            if (Vector3.Distance(target.position, transform.position) <= minRange)
            {
                animator.SetBool("isMoving", false);
                Attack();
            }
            else
            {
                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
                attackedOnce = false;
                hasFollowed = true;
                animator.SetBool("Attack", false);
                animator.SetBool("isMoving", true);
                animator.SetFloat("moveX", (target.position.x - transform.position.x));
                animator.SetFloat("moveY", (target.position.y - transform.position.y));
                rb2d.AddForce(force);
            }
        }

        if (Vector3.Distance(target.position, transform.position) > maxRange && hasFollowed)
        {
            GoHome();
        }

        else if (Vector3.Distance(target.position, transform.position) > maxRange && !hasFollowed)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2d.position).normalized;

            Vector2 force = direction * speed * Time.deltaTime * 100;

            float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            rb2d.AddForce(force);

            if (currentWaypoint >= path.vectorPath.Count)
            {
                animator.SetBool("isMoving", false);
                hitEndOfRoamingPath = true;
            }
            else
            {
                animator.SetBool("Attack", false);
                animator.SetBool("isMoving", true);
                animator.SetFloat("moveX", (path.vectorPath[currentWaypoint].x - transform.position.x));
                animator.SetFloat("moveY", (path.vectorPath[currentWaypoint].y - transform.position.y));
            }
        }
    }

    private IEnumerator WaitForAnimation(Animation animation)
    {
        do
        {
            yield return null;
        } while (animation.isPlaying);
    }

    private void AttackPlayer()
    {
        Player player = FindObjectOfType<Player>();
        Enemy enemy = GetComponent<Enemy>();
        player.GetComponent<PlayerHealth>().subtractHealth(enemy.damage);
    }

    public void Attack()
    {
        time += Time.deltaTime;
        animator.SetBool("Attack", false);

        animator.SetFloat("moveX", (target.position.x - transform.position.x));
        animator.SetFloat("moveY", (target.position.y - transform.position.y));

        if (time >= attackRate || !attackedOnce)
        {
            animator.SetBool("Attack", true);
            AttackPlayer();
            time = 0;
            attackedOnce = true;
        }
    }

    public void GoHome()
    {
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2d.position).normalized;

        Vector2 force = direction * speed * Time.deltaTime * 100;

        float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        rb2d.AddForce(force);

        animator.SetBool("Attack", false);
        animator.SetBool("isMoving", true);
        animator.SetFloat("moveX", (transform.parent.position.x - transform.position.x));
        animator.SetFloat("moveY", (transform.parent.position.y - transform.position.y));

        if (Vector3.Distance(transform.position, transform.parent.position) < 1f)
        {
            animator.SetBool("isMoving", false);
            hasFollowed = false;
        }
    }
}

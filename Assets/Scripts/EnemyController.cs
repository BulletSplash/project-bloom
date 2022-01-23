using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("EnemyBehavior")]
    [SerializeField] private float enemyRange = 5f;
    [SerializeField] private float walkSpeed = 1;
    [SerializeField] private float shootSpeed;
    [SerializeField] private float destroyTime;
    private float shootCounter;
    [SerializeField] private float minTBS = 0.2f;
    [SerializeField] private float maxTBS = 3f;
    private float distanceToTarget;
    private bool isFacingRight;

    [Header("EnemyStats")]
    private int maxHealth;
    private int damage;
    [SerializeField] private int currentHealth;

    [Header("Components")]
    private Rigidbody2D rb;
    [SerializeField] private Transform target, bulletSpawnpoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Stats stat;
    [SerializeField] private BoxCollider2D box2D;
    private Animator animator;
    Vector2 moveDirection;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        maxHealth = stat.MaxHealth();
        currentHealth = maxHealth;
        damage = stat.Damage();
    }
    private void Start()
    {
        shootCounter = Random.Range(minTBS, maxTBS);
    }
    private void Update()
    {
        StartCoroutine(isDetected());
    }
    private void FixedUpdate()
    {
       //StartCoroutine(isDetected());
    }
    IEnumerator isDetected()
    {
        if (PlayerInsight())
        {
            animator.SetBool("Enter", true);
            yield return new WaitForSeconds(1);
            animator.SetBool("AttackPhase", true);
            CountDownAndShoot();
            if (target.position.x > transform.position.x && transform.localScale.x < 0 || target.position.x < transform.position.x && transform.localScale.x > 0)
            {
                Flip();
            }          
        }
        else
        {
            animator.SetBool("Enter", false);
            animator.SetBool("AttackPhase", false);
            yield return new WaitForSeconds(1);
            animator.SetBool("Back", true);

        }
    }
    private void CountDownAndShoot()
    {
        shootCounter -= Time.deltaTime;
        if (shootCounter <= 0f)
        {
            shootCounter = Random.Range(minTBS, maxTBS);
            ShootBullet(destroyTime);
        }
    }
    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
    }
    void ShootBullet(float desTime)
    {
        Vector3 playerPos = new Vector3(target.transform.position.x, target.transform.position.y + 1f, target.transform.position.z);
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnpoint.position, Quaternion.identity);
        moveDirection = ( playerPos - bullet.transform.position);
        moveDirection.Normalize();

        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(moveDirection.x, moveDirection.y) * shootSpeed;
        Destroy(bullet, desTime);
    }

    private bool PlayerInsight()
    {
        RaycastHit2D hit = Physics2D.CircleCast(box2D.bounds.center, enemyRange, Vector2.zero, 0, playerLayer);
        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(box2D.bounds.center, enemyRange);
    }
}

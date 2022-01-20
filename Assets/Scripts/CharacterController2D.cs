using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterController2D : MonoBehaviour
{
    [Header("HorizontalMovement")]
    [SerializeField] private float movSpeed = 20f;
    [SerializeField] private Vector2 direction;
    private bool isFacingRight = true;

    [Header("VerticalMovement")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpPush = 10f;
    [SerializeField] private float jumpDelay = .25f;
    private float jumpTimer;

    [Header("PlayerStats")]
    public int currenthealth = 1;
    private int maxhealth = 1;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private ParticleSystem particleSmoke;
    [SerializeField] Stats[] stat;
    [SerializeField] private GameObject smoke;
    [SerializeField] private GameObject startingpoint;
    [SerializeField] private LayerMask groundLayer;

    [Header("UI")]
    [SerializeField] private Image heartContainer;
    [SerializeField] private Image heartFull;
    [SerializeField] private Image heartHit;


    [Header("Physics")]
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float linearDrag = 4f;
    [SerializeField] private float gravity = 1;
    [SerializeField] private float fallMultiplier = 5f;

    [Header("Collision")]
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private float groundLength = 0.6f;
    [SerializeField] private Vector3 colliderOffset;
    private bool isAlive = true;

    void Awake()
    {
        maxhealth = stat[0].MaxHealth();
        currenthealth = maxhealth;
        heartHit.enabled = false;
        heartContainer.rectTransform.sizeDelta = new Vector2(maxhealth * 100, 100);
        heartFull.rectTransform.sizeDelta = new Vector2(currenthealth * 100, 100);
        heartHit.rectTransform.sizeDelta = new Vector2(currenthealth * 100, 100);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        particleSmoke = smoke.GetComponent<ParticleSystem>();

    }

    private void Update()
    {
        if (!isAlive)
        {
            return;
        }
        isGrounded = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) ||
            Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
        
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
        }
    }

    private void FixedUpdate()
    {
        if (!isAlive)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        MoveCharacter(direction.x);
        if (jumpTimer > Time.time && isGrounded)
        {
            JumpCharacter();
          
        }
        if(direction.x != 0 && isGrounded)
        {
            SpawnDust();
        }
        else
        {
            particleSmoke.Stop();
        }
        modifyPhysics();
        animator.SetFloat("Vertical", rb.velocity.y);
    }
    
    private void MoveCharacter(float horizontal)
    {
        
        rb.AddForce(Vector2.right * horizontal * movSpeed);

        if ((horizontal > 0 && !isFacingRight) || (horizontal < 0 && isFacingRight))
        {
            Flip();
        }

        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);        
        }
        animator.SetFloat("Horizontal", Mathf.Abs(direction.x));
    }
    private void JumpCharacter()
    {
        if (isFacingRight)
        {
            rb.AddForce(Vector2.right * jumpPush, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(-Vector2.right * jumpPush, ForceMode2D.Impulse);
        }
            
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpTimer = 0;
    }
    private void HitHeart(Collision2D collide)
    {
        if (collide.gameObject.tag == "Enemy" || collide.gameObject.tag == "Bullet")
        {
            StartCoroutine(OnHitHeatlth());
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HitHeart(collision);
    }
    IEnumerator OnHitHeatlth()
    {
        heartHit.enabled = true;
        yield return new WaitForSeconds(.1f);
        heartHit.enabled = false;
        yield return new WaitForSeconds(.1f);
        heartHit.enabled = true;
        yield return new WaitForSeconds(.1f);
        currenthealth -= stat[1].Damage();
        heartHit.enabled = false;

        if (currenthealth == 0)
        {
            isAlive = false;
            heartFull.rectTransform.sizeDelta = new Vector2(0 * 100, 100);
            heartHit.rectTransform.sizeDelta = new Vector2(0 * 100, 100);
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            Time.timeScale = .5f;
            yield return new WaitForSeconds(1f);
            Time.timeScale = 1f;
            yield return new WaitForSeconds(.5f);
            RespawnChar();
        }
        heartContainer.rectTransform.sizeDelta = new Vector2(maxhealth * 100, 100);
        heartFull.rectTransform.sizeDelta = new Vector2(currenthealth * 100, 100);
        heartHit.rectTransform.sizeDelta = new Vector2(currenthealth * 100, 100);
    }
    private void RespawnChar()
    {
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        isAlive = true;
        currenthealth = maxhealth;
        transform.position = startingpoint.transform.position;
    }
    private void modifyPhysics()
    {
        animator.SetBool("inGround", isGrounded);
        bool changingDirections =
            (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);
        if (isGrounded)
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * .15f;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localRotation = Quaternion.Euler(0, isFacingRight ? 0 : 180, 0);
    }
    private void SpawnDust()
    {
        particleSmoke.Play();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }
}

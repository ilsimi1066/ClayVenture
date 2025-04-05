using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float horizontalInput;
    float moveSpeed = 5f;
    bool isFacingLeft = false;
    float jumpPower = 6f;
    bool isGrounded = false;
    float direction = 1;
    float normalGravity;
    [SerializeField] playerstate playerstate;
    [SerializeField] GameController gameController;
    public int Getplayerstate()
    {
        return (int) playerstate;
    }

    private bool canDash = true;
    public bool isDashing;
    private float dashingPower = 6f;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 1f;

    Rigidbody2D rb;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        normalGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (horizontalInput != 0)
        {
            direction = horizontalInput;
        }
        horizontalInput = Input.GetAxis("Horizontal");

        FlipSprite();

        if (Input.GetButtonDown("Jump") && isGrounded && playerstate!=0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
        }
        if (Input.GetKeyDown(KeyCode.Q) && canDash && playerstate ==0)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (rb.velocity.y < 0) // Se il giocatore sta scendendo
        {
            rb.gravityScale = normalGravity * 2f; // Aumenta la gravità
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) // Se sta salendo ma non tiene premuto il tasto
        {
            rb.gravityScale = normalGravity * 1.5f; // Leggermente aumentata per farlo scendere più velocemente
        }
        else
        {
            rb.gravityScale = normalGravity; // Reset della gravità normale
        }
    }

    void FlipSprite()
    {
        if (isFacingLeft && horizontalInput < 0f || !isFacingLeft && horizontalInput > 0f)
        {
            isFacingLeft = !isFacingLeft;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        animator.SetBool("isJumping", !isGrounded);

        if (collision.CompareTag("Obstacle"))
        {
            gameController.Die();
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * -1f * dashingPower, 0f);
        // Attiva l'animazione del Dash, ignorando temporaneamente il Blend Tree
        animator.SetBool("isDashing", true);

        yield return new WaitForSeconds(dashingTime);
        // Disattiva il Dash e riattiva il Blend Tree
        animator.SetBool("isDashing", false);

        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
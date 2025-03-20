using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float horizontalInput;
    float moveSpeed = 10f;
    bool isFacingLeft = false;
    float jumpPower = 10f;
    bool isGrounded = false;
    IEnumerator dashCoroutine;
    bool isDashing;
    bool canDash = true;
    float direction = 1;
    float normalGravity;

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

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
        }
        if (Input.GetKeyDown(KeyCode.Q) && canDash == true)
        {
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }
            dashCoroutine = Dash(.2f, 1);
            StartCoroutine(dashCoroutine);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);
        if (isDashing)
        {
            rb.AddForce(new Vector2(direction * 10,0), ForceMode2D.Impulse);
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
    }

    IEnumerator Dash(float dashDuration, float dashCooldown)
    {
        isDashing = true;
        canDash = false;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(direction * 25, 0); // Dash con velocità alta

        // Attiva l'animazione del Dash, ignorando temporaneamente il Blend Tree
        animator.SetBool("isDashing", true);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        rb.gravityScale = normalGravity;
        rb.velocity = Vector2.zero;

        // Disattiva il Dash e riattiva il Blend Tree
        animator.SetBool("isDashing", false);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
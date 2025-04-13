using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float horizontalInput;
    public float moveSpeed = 5f;
    bool isFacingLeft = false;
    float jumpPower = 6f;
    bool isGrounded = false;
    float direction = 1;
    float normalGravity;
    [SerializeField] playerstate playerstate;
    [SerializeField] GameController gameController;
    public int Getplayerstate()
    {
        return (int)playerstate;
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

        // Imposta la velocità iniziale in base allo stato
        if (playerstate == playerstate.Slime)
        {
            moveSpeed = 3f;
        }
        else if (playerstate == playerstate.Mud)
        {
            moveSpeed = 5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (horizontalInput != 0)
        {
            direction = horizontalInput;
        }
        horizontalInput = Input.GetAxis("Horizontal");

        // Premi Z per passare da Mud a Slime e viceversa
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (playerstate == playerstate.Mud)
            {
                SwitchTo(playerstate.Slime);
            }
            else if (playerstate == playerstate.Slime)
            {
                SwitchTo(playerstate.Mud);
            }
        }

        FlipSprite();

        if (Input.GetButtonDown("Jump") && isGrounded && playerstate != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
        }

        // Solo Slime può fare il dash
        if (Input.GetKeyDown(KeyCode.Q) && canDash && playerstate == playerstate.Slime)
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
            rb.gravityScale = normalGravity * 2f; // Aumenta la gravit�
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) // Se sta salendo ma non tiene premuto il tasto
        {
            rb.gravityScale = normalGravity * 1.5f; // Leggermente aumentata per farlo scendere pi� velocemente
        }
        else
        {
            rb.gravityScale = normalGravity; // Reset della gravit� normale
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
    void SwitchTo(playerstate targetState)
    {
        GameController gc = FindObjectOfType<GameController>();
        GameObject target = gc.GetPlayerByIndex((int)targetState);

        if (target != null)
        {
            if (playerstate == playerstate.Slime)
            {
                isDashing = false;
            }

            target.transform.position = transform.position;
            target.SetActive(true);
            this.gameObject.SetActive(false);

            Camera.main.GetComponent<CameraFollow>().target = target.transform;
            gc.currentstate = (int)targetState;

            // 👇 Imposta la velocità direttamente nel nuovo player attivo
            PlayerMovement targetMovement = target.GetComponent<PlayerMovement>();
            if (targetMovement != null)
            {
                if (targetState == playerstate.Slime)
                {
                    targetMovement.moveSpeed = 3f;
                    targetMovement.canDash = true;
                    targetMovement.isDashing = false;
                }
                else if (targetState == playerstate.Mud)
                {
                    targetMovement.moveSpeed = 5f;
                }
            }

        }
    }
}
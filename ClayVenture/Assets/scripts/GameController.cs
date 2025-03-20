using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    Vector2 checkpointPos;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        checkpointPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.CompareTag("Obstacle"))
        {
            if (playerMovement.isDashing)
            {
                Debug.Log("Coccodrillo morto");
            }
            else
            {
                Die();
            }
        }
    }
    public void UpdateCheckpoint(Vector2 pos)
    {
        checkpointPos = pos;
    }

    void Die()
    {
        StartCoroutine(Respawn(0.5f));
    }

    IEnumerator Respawn(float duration)
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(duration);
        transform.position = checkpointPos;
        spriteRenderer.enabled = true;
        //spriteRenderer.sortingOrder = 2;
    }
}

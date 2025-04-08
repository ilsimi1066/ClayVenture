using System.Collections;
using UnityEngine;

enum playerstate
{
    Slime, Mud, Dry
}

public class GameController : MonoBehaviour
{
    [SerializeField] Vector3 startPoint; // Riferimento all'Empty Object
    // Vector2 startPos;
    [SerializeField] GameObject[] players;
    public int currentstate = 0;
    
    private void Start()
    {
        startPoint = transform.position;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Obstacle"))
    //    {
    //        Die();
    //    }
    //}


    public void UpdateCheckpoint(Vector3 pos)
    {
        this.startPoint = pos;
        Debug.Log("Checkpoint updated to: " + startPoint);
    }

    public void Die()
    {
        StartCoroutine(Respawn(0.5f));
    }

    IEnumerator Respawn(float duration)
    {
        SpriteRenderer spriterenderer = players[currentstate].GetComponent<SpriteRenderer>();
        spriterenderer.enabled = false;
        yield return new WaitForSeconds(duration);
        for (int i = 0; i < players.Length; i++)
        {
            players[i].transform.position = startPoint;
        }
        spriterenderer.enabled = true;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    GameController gameController;
    public Transform respawnPoint;

    void Start()
    {
        respawnPoint = this.transform;
        gameController = FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.CompareTag("Player"))
        {
            gameController.UpdateCheckpoint(new Vector2(respawnPoint.position.x, respawnPoint.position.y));
        }
    }
}
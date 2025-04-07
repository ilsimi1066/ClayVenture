using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] GameController gameController;
    public Transform respawnPoint;

    void Start()
    {
        respawnPoint = this.transform;
        gameController = FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log(collision.gameObject.tag);
            gameController.UpdateCheckpoint(respawnPoint.position);
        }
    }
}
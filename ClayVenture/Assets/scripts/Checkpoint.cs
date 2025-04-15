using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] GameController gameController;
    public Transform respawnPoint;
    Animator animator;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        respawnPoint = this.transform;
        gameController = FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.checkpoint);
            animator.SetBool("isCheck", true);
            Debug.Log(collision.gameObject.tag);
            gameController.UpdateCheckpoint(respawnPoint.position);
        }
    }
}
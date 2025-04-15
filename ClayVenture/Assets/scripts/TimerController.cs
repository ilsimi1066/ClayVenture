using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public Image timer_linear_image;
    float time_remaining;
    public float max_time = 5.0f;

    [SerializeField] GameController gameController;
    [SerializeField] GameObject gameOverUI; // Riferimento all'UI di Game Over

    private bool gameOver = false; // Flag per determinare se il gioco è finito
    private playerstate? previousState = null;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        time_remaining = max_time;
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false); // Assicurati che l'UI di Game Over sia disattivato all'inizio
        }
    }

    void Update()
    {
        // Se il gioco è già finito, non eseguire altre operazioni
        if (gameOver)
        {
            return; // Se è game over, non eseguire altre azioni
        }

        GameObject currentPlayer = gameController.GetPlayerByIndex(gameController.currentstate);

        if (currentPlayer != null)
        {
            PlayerMovement playerMovement = currentPlayer.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                playerstate currentState = (playerstate)playerMovement.Getplayerstate();

                // Se il giocatore è in stato Dry, decresce il timer
                if (currentState == playerstate.Dry)
                {
                    if (time_remaining > 0)
                    {
                        time_remaining -= Time.deltaTime;
                        timer_linear_image.fillAmount = time_remaining / max_time;
                    }
                    else
                    {
                        GameOver(); // Chiamato quando il timer arriva a zero
                    }
                }

                // Se passa da Dry a Mud, resetta il timer
                if (previousState == playerstate.Dry && currentState == playerstate.Mud)
                {
                    ResetTimer();
                }

                previousState = currentState;
            }
        }
    }

    void ResetTimer()
    {
        time_remaining = max_time;
        timer_linear_image.fillAmount = 1f;
    }

    void GameOver()
    {
        if (!gameOver) // Evita chiamate multiple per GameOver
        {
            gameOver = true; // Imposta il flag per evitare azioni successive
            if (gameOverUI != null)
            {
                audioManager.PlaySFX(audioManager.gameover);

                gameOverUI.SetActive(true); // Mostra l'UI di Game Over
            }
            Time.timeScale = 0f; // Imposta il gioco in pausa
        }
    }
}


using UnityEngine;

public class FogliaDialogo : MonoBehaviour
{
    public GameObject dialogoUI; // Il Panel che contiene tutto

    private void Start()
    {
        if (dialogoUI != null)
        {
            dialogoUI.SetActive(false); // Nasconde il panel all'inizio
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Il Player è entrato nel trigger!");

            if (dialogoUI != null)
            {
                dialogoUI.SetActive(true); // Attiva il panel + testo
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Il Player è uscito dal trigger!");

            if (dialogoUI != null)
            {
                dialogoUI.SetActive(false); // Nasconde tutto
            }
        }
    }
}
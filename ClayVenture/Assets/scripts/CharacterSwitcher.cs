using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    public GameObject playerOld;  // Il personaggio attuale
    public GameObject playerNew;  // Il nuovo personaggio

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))  // Assicurati che il Player abbia il tag corretto
        {
            SwitchCharacter();
        }
    }

    void SwitchCharacter()
    {
        // Disattiva il vecchio personaggio
        playerOld.SetActive(false);

        // Attiva il nuovo personaggio
        playerNew.SetActive(true);

        Vector3 asd = new Vector3(0, 1);

        playerNew.transform.position = playerOld.transform.position + asd;
    }
}

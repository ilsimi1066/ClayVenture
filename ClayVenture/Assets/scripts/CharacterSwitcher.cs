using System.Collections;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    public GameObject playerOld;  // Il personaggio attuale
    public GameObject playerNew;  // Il nuovo personaggio
    private bool hasSwitched = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasSwitched && collision.CompareTag("Player"))
        {
            hasSwitched = true;
            SwitchCharacter();
        }
    }

    void SwitchCharacter()
    {
        // Disattiva il vecchio personaggio
        playerOld.SetActive(false);

        // Attiva il nuovo personaggio
        playerNew.SetActive(true);

        Vector3 Change = new Vector3(0, 1.5f);  // Aumenta la distanza verticale
        playerNew.transform.position = playerOld.transform.position + Change;


        // Aggiorna il target della camera
        Camera.main.GetComponent<CameraFollow>().target = playerNew.transform;

        Collider2D newCollider = playerNew.GetComponent<Collider2D>();
        if (newCollider != null)
        {
            StartCoroutine(ResetCollider(newCollider));
        }
    }
     private IEnumerator ResetCollider(Collider2D col)
    {
        col.enabled = false;
        yield return new WaitForSeconds(0.2f); // Aspetta un po' prima di riattivarlo
        col.enabled = true;
    }
}

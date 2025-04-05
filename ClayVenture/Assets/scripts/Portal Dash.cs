using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalDash : MonoBehaviour
{
    public HashSet<GameObject> portalObjects = new HashSet<GameObject>();
    [SerializeField] private Transform destination;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Controlla se l'oggetto che entra è un giocatore con lo script Slime
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

        if (player == null || !player.isDashing)
        {
            return; // Esce se il giocatore non sta dashando
        }

        if (portalObjects.Contains(collision.gameObject))
        {
            return;
        }

        if (destination.TryGetComponent(out Portal destinationPortal))
        {
            destinationPortal.portalObjects.Add(collision.gameObject);
        }

        collision.transform.position = destination.position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        portalObjects.Remove(collision.gameObject);
    }
}
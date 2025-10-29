using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   public Transform player;           // Il giocatore da seguire
    public Vector3 offset;             // La distanza tra la fotocamera e il giocatore
    public float smoothSpeed = 0.125f; // La velocità con cui la fotocamera segue il giocatore

    void Start()
    {
        // Assicurati che la fotocamera abbia una distanza iniziale dal giocatore
        if (player != null)
        {
            offset = transform.position - player.position;
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            // Calcola la nuova posizione della fotocamera
            Vector3 desiredPosition = player.position + offset;

            // Muovi la fotocamera in modo fluido verso la nuova posizione
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            
            // Aggiorna la posizione della fotocamera
            transform.position = smoothedPosition;

            // Se vuoi che la fotocamera guardi sempre il giocatore:
            transform.LookAt(player);
        }
    }
}

using UnityEngine;

public class Token : MonoBehaviour
{
    public int value = 1; // Value of the token

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Notify the player script about the collected token
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.CollectToken(value);
                // Destroy the token
                Destroy(gameObject);
            }
        }
    }
}

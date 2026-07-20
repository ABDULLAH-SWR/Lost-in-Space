using UnityEngine;

public class BreakableObstacle : MonoBehaviour
{
    [Header("Smash Settings")]
    [Tooltip("How much mass the player needs to break this object.")]
    public float requiredMassToBreak = 2.0f; // Assuming Normal is 1, and Anvil is heavier (e.g., 5 or 10)

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the thing hitting us is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            // If the player exists and is heavy enough...
            if (playerRb != null && playerRb.mass >= requiredMassToBreak)
            {
                Debug.Log("Obstacle Smashed!");

                // Optional: You could instantiate a breaking glass particle effect here later!

                // Destroy the glass block
                Destroy(gameObject);
            }
        }
    }
}
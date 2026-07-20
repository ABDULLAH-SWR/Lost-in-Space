using UnityEngine;

/// <summary>
/// Handles collision detection for the player, triggering level win or loss conditions.
/// </summary>
public class PlayerCollision : MonoBehaviour
{
    [Tooltip("Reference to the LevelManager to reload scenes on win/loss.")]
    public LevelManager levelManager;

    private void Start()
    {
        // Automatically find the LevelManager if the prefab lost its reference
        if (levelManager == null)
        {
            levelManager = FindObjectOfType<LevelManager>();
        }

        // Warn if it still cannot find the manager in the scene
        if (levelManager == null)
        {
            Debug.LogError("[PlayerCollision] Could not find a LevelManager in this scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Target"))
        {
            // --- THE FIX: Freeze the player in place immediately ---
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; // Stops all directional movement
                rb.angularVelocity = 0f;          // Stops all spinning
                rb.bodyType = RigidbodyType2D.Kinematic; // Turns off gravity and bounces
            }

            if (levelManager != null)
            {
                levelManager.TriggerWin();
            }
            else
            {
                Debug.LogError("[PlayerCollision] LevelManager reference is null!");
            }
        }
        else if (other.CompareTag("OutOfBounds") || other.CompareTag("Hazard"))
        {
            if (levelManager != null)
            {
                levelManager.RegisterMiss();
            }
            else
            {
                Debug.LogError("[PlayerCollision] LevelManager reference is null!");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard") || collision.gameObject.CompareTag("OutOfBounds"))
        {
            if (SoundManager.instance != null) SoundManager.instance.PlaySpikeBurst();
            if (levelManager != null) levelManager.RegisterMiss();
        }
        else
        {
            if (SoundManager.instance != null) SoundManager.instance.PlayWallBounce();
        }
    }
}
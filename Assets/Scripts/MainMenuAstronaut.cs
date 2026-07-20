using UnityEngine;

public class MainMenuAstronaut : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The speed at which the astronaut floats.")]
    public float moveSpeed = 3f;

    [Tooltip("How fast the astronaut rotates as it floats.")]
    public float rotationSpeed = 15f;

    [Tooltip("A little padding to make the bounce feel natural near the edge.")]
    public float screenMargin = 0.5f;

    private Vector2 velocity;
    private Camera mainCam;
    private Vector2 screenBounds;

    void Start()
    {
        mainCam = Camera.main;

        // 1. Calculate the World Space coordinates of the screen edges
        CalculateScreenBounds();

        // 2. Set an initial random direction and speed
        InitializeRandomDirection();
    }

    void Update()
    {
        // 1. Move the astronaut based on its current velocity
        transform.Translate(velocity * Time.deltaTime, Space.World);

        // 2. Apply a slow, gentle rotation (aesthetic)
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // 3. Detect when it hits the "walls" and make it bounce
        CheckAndHandleBouncing();
    }

    void CalculateScreenBounds()
    {
        // Get the top-right corner of the screen in World coordinates
        Vector2 upperRight = mainCam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        // Use that point and the margin to set our bounding box
        screenBounds = new Vector2(upperRight.x - screenMargin, upperRight.y - screenMargin);
    }

    void InitializeRandomDirection()
    {
        // Generate a random angle, then convert that angle into a Vector2 velocity
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 randomDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));

        velocity = randomDirection.normalized * moveSpeed;
    }

    void CheckAndHandleBouncing()
    {
        // Hit the LEFT wall or RIGHT wall?
        if (transform.position.x < -screenBounds.x || transform.position.x > screenBounds.x)
        {
            velocity.x *= -1f; // Reverse the horizontal direction

            // Apply a slight random rotation kick on bounce (aesthetic)
            rotationSpeed = Random.Range(-25f, 25f);
        }

        // Hit the TOP wall or BOTTOM wall?
        if (transform.position.y < -screenBounds.y || transform.position.y > screenBounds.y)
        {
            velocity.y *= -1f; // Reverse the vertical direction

            // Apply a slight random rotation kick on bounce (aesthetic)
            rotationSpeed = Random.Range(-25f, 25f);
        }
    }

    // Safety check: If the screen size changes, recalculate bounds
    void OnRectTransformDimensionsChange()
    {
        if (mainCam != null) CalculateScreenBounds();
    }
}
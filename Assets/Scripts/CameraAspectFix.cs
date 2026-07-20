using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAspectFix : MonoBehaviour
{
    [Tooltip("The aspect ratio you designed the game for (usually 16:9)")]
    public Vector2 targetAspect = new Vector2(16, 9);

    private Camera cam;
    private float defaultOrthographicSize;

    void Start()
    {
        cam = GetComponent<Camera>();
        defaultOrthographicSize = cam.orthographicSize;
        AdjustCamera();
    }

    void AdjustCamera()
    {
        // Calculate the target ratio (16/9 = 1.777)
        float targetRatio = targetAspect.x / targetAspect.y;

        // Calculate the actual screen ratio of the web browser
        float windowRatio = (float)Screen.width / (float)Screen.height;

        // Compare the two
        float scaleHeight = windowRatio / targetRatio;

        // If the screen is narrower than 16:9, zoom the camera out
        if (scaleHeight < 1.0f)
        {
            cam.orthographicSize = defaultOrthographicSize / scaleHeight;
        }
        else
        {
            // If it's wider (like an ultrawide monitor), keep the default size
            cam.orthographicSize = defaultOrthographicSize;
        }
    }
}
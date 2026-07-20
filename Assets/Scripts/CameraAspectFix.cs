using UnityEngine;

public class AspectRatioFitter : MonoBehaviour
{
    // Set your target aspect ratio here (16:9)
    public float targetAspect = 16.0f / 9.0f;

    void Start()
    {
        // Determine the current screen's aspect ratio
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // Compare current ratio to target ratio
        float scaleHeight = windowAspect / targetAspect;

        Camera cam = GetComponent<Camera>();

        // If the screen is taller than 16:9 (like your 16-inch laptop)
        if (scaleHeight < 1.0f)
        {
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            cam.rect = rect;
        }
        // If the screen is wider than 16:9 (like an ultrawide monitor)
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            cam.rect = rect;
        }
    }
}
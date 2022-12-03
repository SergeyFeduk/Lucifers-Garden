using UnityEngine;

public static class ScreenUtils
{
    public static Camera camera = null;
    /// <summary>
    /// Returns world position of cursor based on camera
    /// </summary>
    public static Vector2 GetWorldMousePosition() {
        if (camera == null) { CacheCamera(Camera.main); }
        return camera.ScreenToWorldPoint(Input.mousePosition);
    }
    public static Vector2 GetWorldPosition(Vector2 position) {
        if (camera == null) { CacheCamera(Camera.main); }
        return camera.ScreenToWorldPoint(position);
    }
    public static Vector2 GetWorldPosition(float x, float y)
    {
        if (camera == null) { CacheCamera(Camera.main); }
        return camera.ScreenToWorldPoint(new Vector2(x,y));
    }
    public static bool IsPositionOnScreen(Vector2 position) {
        if (camera == null) { CacheCamera(Camera.main); }
        Vector2 screenPosition = camera.WorldToScreenPoint(position);
        return screenPosition.x > 0 && screenPosition.x < camera.pixelWidth 
            && screenPosition.y > 0 && screenPosition.y < camera.pixelHeight;
    }

    /// <summary>
    /// Caches camera for faster access
    /// </summary>
    /// <param name="newCamera">Current camera object to cache</param>
    public static void CacheCamera(Camera newCamera) {
        camera = newCamera;
    }
}
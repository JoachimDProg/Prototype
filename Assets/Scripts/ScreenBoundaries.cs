using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBoundaries : MonoBehaviour
{
    [Header("Stage Limit")]
    [SerializeField] private float xPadding;
    [SerializeField] private float yPadding;
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    public Bounds screenBounds;
    public static ScreenBoundaries Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        float verticalSize = yMax - yMin;
        float horizontalSize = xMax - xMin;

        screenBounds = new Bounds(new Vector3(0,0,0), new Vector3(horizontalSize, verticalSize, 0));
        // Debug.DrawLine(screenBounds.min, screenBounds.max, Color.red, 20);
    }

    public Vector2 ClampPosition(Vector2 position)
    {
        float xLimit = Mathf.Clamp(position.x, xMin + xPadding, xMax - xPadding);
        float yLimit = Mathf.Clamp(position.y, yMin + yPadding, yMax - yPadding);

        return new Vector2(xLimit, yLimit);
    }

    public bool IsOutOfBounds(Vector3 position)
    {
        return !screenBounds.Contains(position);
    }

    public bool IsOutOfBounds(Vector3 position, float padding)
    {
        // distance from bound + padding
        return !screenBounds.Contains(position);
    }

    public float DistanceFromBounds(Vector3 position)
    {
        return Vector3.Distance(screenBounds.ClosestPoint(position), position);
    }
}

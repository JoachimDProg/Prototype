using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBoundaries : MonoBehaviour
{
    [Header("Stage Limit")]
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;
    
    public Bounds screenBounds;
    public static ScreenBoundaries Instance;

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
    }

    public Vector2 ClampPlayerPosition(Vector2 position, Vector2 playerSpriteSize)
    {
        float xLimit = Mathf.Clamp(position.x, xMin + playerSpriteSize.x, xMax - playerSpriteSize.x);
        float yLimit = Mathf.Clamp(position.y, yMin + playerSpriteSize.y, yMax - playerSpriteSize.y);

        return new Vector2(xLimit, yLimit);
    }

    public bool IsOutOfBounds(Vector3 position)
    {
        return !screenBounds.Contains(position);
    }

    public float DistanceFromBounds(Vector3 position)
    {
        return Vector3.Distance(screenBounds.ClosestPoint(position), position);
    }
}

using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // objects references
    [SerializeField] private Player player = null;
    [SerializeField] private GameObject initialPoint = null;
    [SerializeField] private GameObject[] points = null;
    [SerializeField] AnimationCurve animationCurve = null;

    [Header("Enemy Configuration")]
    [SerializeField] private float movementRate = 0.0f;
    [SerializeField] private float movementSpeed = 0.0f;
    [SerializeField] private float movementTime = 0.0f;
    [SerializeField] private float shootRange = 0.0f;

    [Header("Observation Variable")]
    [SerializeField] private Gun[] guns;
    private Vector3 playerPosition;
    private float movementCooldownTimer;

    // Lerp variables
    [SerializeField] float time = 0.0f;
    bool hasEnteredScene = false;
    private int destinationPointIndex;
    private Vector2 initialPointPosition;
    private Vector2 currentPointPosition;
    private Vector2 destinationPointPosition;

    private void Start()
    {
        guns = GetComponentsInChildren<Gun>();
        movementCooldownTimer = 0;

        // Lerp init
        destinationPointIndex = 1;
        initialPointPosition = initialPoint.transform.position;
        currentPointPosition = points[0].transform.position;
        destinationPointPosition = points[1].transform.position;

        InitMove();
    }

    private void Update()
    {
        if (!hasEnteredScene)
            EnterScene();
        else
            Move();
    }

    private void InitMove()
    {
        transform.position = initialPointPosition;
    }

    private void Move()
    {
        if (time < movementTime)
        {
            float t = time / movementTime;
            t = Mathf.Clamp01(t);
            t = animationCurve.Evaluate(t);

            transform.position = Vector2.Lerp(currentPointPosition, destinationPointPosition, t);

            time += Time.deltaTime;

            if (time > movementTime)
            {
                StartCoroutine(WaitBetweenMoves());
            }
        }
    }

    private void EnterScene()
    {
        float t = time / movementTime;

        time += Time.deltaTime;

        transform.position = Vector2.Lerp(initialPointPosition, currentPointPosition, t);

        if (transform.position == points[0].transform.position)
        {
            StartCoroutine(BossWait());
        }
    }

    IEnumerator BossWait()
    {
        yield return new WaitForSeconds(1);

        hasEnteredScene = true;
        time = 0.0f;
    }

    IEnumerator WaitBetweenMoves()
    {
        Shoot();

        yield return new WaitForSeconds(1);

        destinationPointIndex++;
        if (destinationPointIndex >= points.Length)
            destinationPointIndex = 0;

        // reset times
        time = 0.0f;

        // go to next destination
        currentPointPosition = destinationPointPosition;
        destinationPointPosition = points[destinationPointIndex].transform.position;
    }

    private void Shoot()
    {
        foreach (Gun gun in guns)
        {
            gun.Shoot();
        }
    }

    public Vector2 CalculateQuadBezier(Vector2 point1, Vector2 point2, Vector2 point3, float t)
    {
        float oneMinT = 1 - t;
        float oneMinTT = oneMinT * oneMinT;
        float TxT = t * t;

        return oneMinTT * point1 + 2 * oneMinT * t * point2 + TxT * point3;
    }
}

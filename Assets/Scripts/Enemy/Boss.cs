using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private GameObject initialPoint;
    [SerializeField] private GameObject[] points;
    private Gun[] guns;

    [Header("Movement Configuration")]
    [SerializeField] private float movementSpeedInSeconds = 0.0f;
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private float waitBetweenMoves;

    [Header("Lerp Variables")]
    private float time = 0.0f;
    private bool hasEnteredScene = false;
    private int destinationPointIndex;
    private Vector2 initialPointPosition;
    private Vector2 currentPointPosition;
    private Vector2 destinationPointPosition;

    private void Awake()
    {
        guns = GetComponentsInChildren<Gun>();
    }

    private void Start()
    {
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
        if (time < movementSpeedInSeconds)
        {
            float t = time / movementSpeedInSeconds;
            t = Mathf.Clamp01(t);
            t = movementCurve.Evaluate(t);

            transform.position = Vector2.Lerp(currentPointPosition, destinationPointPosition, t);

            time += Time.deltaTime;

            if (time > movementSpeedInSeconds)
            {
                StartCoroutine(WaitBetweenMoves());
            }
        }
    }

    private void EnterScene()
    {
        float t = time / movementSpeedInSeconds;

        time += Time.deltaTime;

        transform.position = Vector2.Lerp(initialPointPosition, currentPointPosition, t);

        if (transform.position == points[0].transform.position)
        {
            StartCoroutine(BossWait());
        }
    }

    IEnumerator BossWait()
    {
        yield return new WaitForSeconds(waitBetweenMoves);

        hasEnteredScene = true;
        time = 0.0f;
    }

    IEnumerator WaitBetweenMoves()
    {
        Shoot();

        yield return new WaitForSeconds(waitBetweenMoves);

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
}

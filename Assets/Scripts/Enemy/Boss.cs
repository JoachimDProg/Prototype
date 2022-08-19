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
    private Gun gun;
    private Vector3 initialPosition;
    private Vector3 playerPosition;
    private float movementCooldownTimer;

    // Lerp variables
    float time = 0.0f;
    bool hasEnteredScene = false;
    private float movementTimeInitial;
    private int destinationPointIndex;
    private Vector2 initialPointPosition;
    private Vector2 currentPointPosition;
    private Vector2 destinationPointPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        gun = GetComponentInChildren<Gun>();
        movementCooldownTimer = 0;

        // Lerp init
        movementTimeInitial = movementTime;
        destinationPointIndex = 1;
        initialPointPosition = initialPoint.transform.position;
        currentPointPosition = points[0].transform.position;
        destinationPointPosition = points[1].transform.position;

        InitMove();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasEnteredScene)
            EnterScene();
        else
            Move();
        //Shoot();
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
                destinationPointIndex++;
                if (destinationPointIndex >= points.Length)
                    destinationPointIndex = 0;

                // reset times
                //movementTime = movementTimeInitial;
                time = 0;

                // go to next destination
                currentPointPosition = destinationPointPosition;
                destinationPointPosition = points[destinationPointIndex].transform.position;

                /// TODO wait between point
                //StartCoroutine(WaitBetweenMoves());
            }
            Debug.Log(t);
        }

        // decrease cooldown each frame
        //movementCooldownTimer -= Time.deltaTime;
    }

    private void EnterScene()
    {
        float t = time / movementTime;
        t = Mathf.Clamp01(t);

        time += Time.deltaTime;

        transform.position = Vector2.Lerp(initialPointPosition, currentPointPosition, t);

        if (transform.position == points[0].transform.position)
        {
            StartCoroutine(BossWait());
        }
    }

    IEnumerator BossWait()
    {
        /// TODO wait is more than 1 sec
        yield return new WaitForSecondsRealtime(1);

        hasEnteredScene = true;
        time = 0;
    }

    IEnumerator WaitBetweenMoves()
    {
        yield return new WaitForSeconds(1);
    }

    private void Shoot()
    {
        // if player is within a certain range in the x axis
        if (Vector2.Distance(new Vector3(transform.position.x, 0), new Vector3(player.transform.position.x, 0)) < shootRange)
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

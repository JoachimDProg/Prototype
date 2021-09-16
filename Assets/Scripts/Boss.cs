using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // objects references
    [SerializeField] private Player player;
    [SerializeField] private GameObject[] points;
    float time = 0;
    [SerializeField] AnimationCurve animationCurve;

    [Header("Enemy Configuration")]
    [SerializeField] private float movementRate;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementTime;
    [SerializeField] private float shootRange;

    [Header("Observation Variable")]
    private Gun gun;
    private Vector3 initialPosition;
    private Vector3 playerPosition;
    private float movementCooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        gun = GetComponentInChildren<Gun>();
        movementCooldownTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        InitMove();
        Move();
        Shoot();
    }

    private void InitMove()
    {
        // if cooldown is finished, reset cooldown and register current x player position
        if (movementCooldownTimer <= 0)
        {
            movementCooldownTimer = movementRate;
            playerPosition = new Vector3(player.transform.position.x, initialPosition.y, 0);
        }

        Move();
    }

    private void Move()
    {
        /*if (Vector2.Distance(new Vector3(transform.position.x, 0), new Vector3(player.transform.position.x, 0)) < playerRange)
        {
            playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, 0);
        }*/

        // move towards player position each frame
        // transform.position = Vector3.MoveTowards(transform.position, playerPosition, movementSpeed * Time.deltaTime);

        if (time <= movementTime)
        {
            // Lerp Move with ease
            // float newt = time < 0.5 ? 2 * time * time * time : 1 - Mathf.Pow(-2 * time + 2, 3) / 2;
            // float newt = time * time * time;
            // transform.position = Vector3.Lerp(points[0].transform.position, points[1].transform.position, newt / movementSpeed);

            // transform.position = CalculateQuadBezier(points[0].transform.position, points[1].transform.position, points[2].transform.position, newt / 60);
            float t = time / movementTime;
            t = Mathf.Clamp01(t);
            t = animationCurve.Evaluate(t);

            transform.position = Vector2.Lerp(Vector2.Lerp(points[0].transform.position, points[1].transform.position, t),
                                              Vector2.Lerp(points[1].transform.position, points[2].transform.position, t),
                                              t);
            time += Time.deltaTime;
        }

        // decrease cooldown each frame
        movementCooldownTimer -= Time.deltaTime;
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

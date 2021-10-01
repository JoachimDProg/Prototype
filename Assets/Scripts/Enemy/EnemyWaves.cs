using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaves : MonoBehaviour
{
    [Header("Wave Movement Parameters")]
    [SerializeField] private float moveSpeed = 0f;

    [Header("Wave Configuration")]
    [SerializeField] private Queue<Enemy> enemyBase;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private int population = 0;
    [SerializeField] private float dequeueTimer = 0f;
    private float canEmptyDistance = 5f;
    private bool canEmptyPool = false;
    private float sendTroopsTimer = 0f;

    [Header("Wave Movement Configuration")]
    private IMovement waveMovement;
    [SerializeField] private float waveSpeed = 0f;

    private Dictionary<string, float> sineParam;
    [SerializeField] private bool isSine = false;
    [SerializeField] private bool isInverted = false;
    [SerializeField] private float amplitude = 0f;
    [SerializeField] private float frequency = 0f;
    [SerializeField] private float offset = 0f;

    void Start()
    {
        sendTroopsTimer = dequeueTimer;
        FillBase();
        InitSineParam();
        WaveMoveInit(isSine);
    }

    void Update()
    {
        Move();
        HasArrivedToBounds(transform.position);
        EmptyBase(canEmptyPool);
    }

    private void Move()
    {
        Vector2 velocity;
        velocity = transform.up * moveSpeed;
        transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;
    }

    private void HasArrivedToBounds(Vector3 position)
    {
        if (ScreenBoundaries.Instance.DistanceFromBounds(position) < canEmptyDistance)
        {
            moveSpeed = 0;
            canEmptyPool = true;
        }
    }

    private void FillBase()
    {
        enemyBase = new Queue<Enemy>();

        for (int i = 0; i < population; i++)
        {
            Enemy enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.gameObject.SetActive(false);
            enemyBase.Enqueue(enemy);
        }
    }

    private void EmptyBase(bool canStart)
    {
        if (canStart)
        {
            sendTroopsTimer -= Time.deltaTime;

            if (sendTroopsTimer <= 0)
            {
                Enemy enemy = enemyBase.Dequeue();
                enemy.InitParameters(transform.position, transform.up, waveSpeed, ReturnToBase);
                enemy.InitMove(WaveMoveInit(isSine), isSine, sineParam);
                enemy.gameObject.SetActive(true);
                sendTroopsTimer = dequeueTimer;
            }

            if (enemyBase.Count == 0)
                gameObject.SetActive(false);
        }
    }

    private IMovement WaveMoveInit(bool sine)
    {
        if (sine)
            return new SineMove();
        else
            return new NormalMove();
    }

    private void InitSineParam()
    {
        sineParam = new Dictionary<string, float>();
        int inverted = 1;

        sineParam.Add("frequency", frequency);
        sineParam.Add("amplitude", amplitude);
        sineParam.Add("offset", offset);
        if (isInverted) inverted = -1;
        sineParam.Add("inverted", inverted);
    }

    private void ReturnToBase(Enemy enemy)
    {
        enemyBase.Enqueue(enemy);
        enemy.gameObject.SetActive(false);
    }
}

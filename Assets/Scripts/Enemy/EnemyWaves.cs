using System.Collections.Generic;
using UnityEngine;

public class EnemyWaves : MonoBehaviour
{
    [Header("Wave Movement Configuration")]
    [SerializeField] private float moveSpeed = 0f;

    [Header("Enemy Wave Configuration")]
    [SerializeField] private Queue<Enemy> enemyPool;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private int population = 0;
    [SerializeField] private float dequeueTimer = 0f;
    [SerializeField] private float waveSpeed = 0f;
    private bool canEmptyPool = false;
    private float canEmptyDistance = 5f;
    private float sendEnemyTimer = 0f;
    
    public enum MovementType { Normal, Seek, Sine };
    [HideInInspector] public MovementType movementType;
    [HideInInspector] public int moveTypeFlag = 0;

    [HideInInspector] public bool isSine = false;
    [HideInInspector] public bool isInverted = false;
    [HideInInspector] public float amplitude = 0.0f;
    [HideInInspector] public float frequency = 0.0f;
    [HideInInspector] public float offset = 0.0f;

    // container to hold and pass parameters to enemy object
    Dictionary<string, float> sineParam;

    void Start()
    {
        sendEnemyTimer = dequeueTimer;
        FillBase();
    }

    void Update()
    {
        Move();
        HasArrivedToBounds(transform.position);
        EmptyBase(canEmptyPool);
    }

    private void Move()
    {
        Vector2 velocity = transform.up * moveSpeed;
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
        enemyPool = new Queue<Enemy>();

        for (int i = 0; i < population; i++)
        {
            Enemy enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.InitMove(WaveMoveInit(), isSine, sineParam);
            enemy.gameObject.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }

    private void EmptyBase(bool canEmpty)
    {
        if (canEmpty)
        {
            sendEnemyTimer -= Time.deltaTime;

            if (sendEnemyTimer <= 0)
            {
                Enemy enemy = enemyPool.Dequeue();
                enemy.InitParameters(transform.position, transform.up, waveSpeed, ReturnToBase);
                enemy.gameObject.SetActive(true);
                sendEnemyTimer = dequeueTimer;
            }

            if (enemyPool.Count == 0)
                gameObject.SetActive(false);
        }
    }

    private IMovement WaveMoveInit()
    {
        if (moveTypeFlag == 0)
            return new NormalMove();
        else if (moveTypeFlag == 1)
            return new SeekMove();
        else if (moveTypeFlag == 2)
        {
            InitSineParam();
            return new SineMove();
        }
        else
            return new NormalMove();
    }

    private void InitSineParam()
    {
        sineParam = new Dictionary<string, float>();
        int inverted = isInverted ? -1 : 1;

        sineParam.Add("frequency", frequency);
        sineParam.Add("amplitude", amplitude);
        sineParam.Add("offset", offset);
        sineParam.Add("inverted", inverted);
    }

    private void ReturnToBase(Enemy enemy)
    {
        enemyPool.Enqueue(enemy);
        enemy.gameObject.SetActive(false);
    }
}

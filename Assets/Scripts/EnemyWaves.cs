using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaves : MonoBehaviour
{
    [Header("Wave Movement Parameters")]
    [SerializeField] private float moveSpeed; 
    private Vector2 velocity; // mettre dans move?

    [Header("Wave Configuration")]
    [SerializeField] private Queue<Enemy> enemyBase;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private int population;
    [SerializeField] private float dequeueTimer;
    private bool canEmpty = false;
    private float sendTroopsTimer;

    [Header("Wave Movement Configuration")]
    [SerializeField] protected float waveSpeed;

    [SerializeField] protected bool sine;
    [SerializeField] protected bool inverted;
    [SerializeField] protected float amplitude;
    [SerializeField] protected float frequency;
    [SerializeField] protected float offset;
    
    void Start()
    {
        sendTroopsTimer = dequeueTimer;
        FillBase();
    }

    void Update()
    {
        Move();
        EmptyBase(canEmpty);
    }

    private void Move()
    {
        velocity = transform.up * moveSpeed;
        transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        moveSpeed = 0;
        canEmpty = true;
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
                if (sine)
                    enemy.InitSine(sine, inverted, amplitude, frequency, offset);
                enemy.gameObject.SetActive(true);
                sendTroopsTimer = dequeueTimer;
            }

            if (enemyBase.Count == 0)
                gameObject.SetActive(false);
        }
    }

    protected void ReturnToBase(Enemy enemy)
    {
        enemyBase.Enqueue(enemy);
        enemy.gameObject.SetActive(false);
    }
}

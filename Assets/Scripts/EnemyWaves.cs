using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaves : MonoBehaviour
{
    [SerializeField] private Queue<Enemy> enemyBase;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private int population;
    [SerializeField] private float dequeueTimer;
    private float sendTroopsTimer;

    // Start is called before the first frame update
    void Start()
    {
        sendTroopsTimer = dequeueTimer;
        FillBase();
    }

    // Update is called once per frame
    void Update()
    {
        EmptyBase();
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

    private void EmptyBase()
    {
        sendTroopsTimer -= Time.deltaTime;

        if (sendTroopsTimer <= 0)
        {
            Enemy enemy = enemyBase.Dequeue();
            enemy.InitParameters(transform.position, transform.up, ReturnToBase);
            enemy.gameObject.SetActive(true);
            sendTroopsTimer = dequeueTimer;
        }

        if (enemyBase.Count == 0)
            gameObject.SetActive(false);
    }

    protected void ReturnToBase(Enemy enemy)
    {
        enemyBase.Enqueue(enemy);
        enemy.gameObject.SetActive(false);
    }
}

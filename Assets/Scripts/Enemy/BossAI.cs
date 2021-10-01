using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    private Vector2 startPosition;
    private Vector2 roamPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        roamPosition = GetRoamPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetRoamPosition()
    {
        Vector2 randomVector = new Vector2(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1)).normalized;

        return startPosition + randomVector * UnityEngine.Random.Range(10f, 50f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Shoot()
    {
        
    }

    public override void Move()
    {
        base.Move();
        if (transform.position.y < player.transform.position.x)
            enemyMovement = new NormalMove();
    }
}

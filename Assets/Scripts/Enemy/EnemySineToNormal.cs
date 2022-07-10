using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySineToNormal : Enemy
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
        if (transform.position.y < player.transform.position.y)
            movement = new NormalMove();
    }
}

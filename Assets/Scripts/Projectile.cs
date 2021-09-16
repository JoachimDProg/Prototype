using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [Header("Projectile Configuration")]
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected int projectileDamage;

    // variables passed from parent
    protected Vector2 velocity;
    private Action<Projectile> returnToPool;

    void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        returnToPool.Invoke(this);
    }

    protected virtual void Move()
    {
        transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;

        if (ScreenBoundaries.Instance.DistanceFromBounds(transform.position) > 1f)
        {
            returnToPool.Invoke(this);
        }
    }

    public void InitParameters(Vector2 position, Vector2 direction, Action<Projectile> returnToPool)
    {
        transform.position = position;
        Vector3 eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, direction));
        transform.eulerAngles = eulerAngles;
        velocity = direction * projectileSpeed;

        this.returnToPool = returnToPool;
    }
}

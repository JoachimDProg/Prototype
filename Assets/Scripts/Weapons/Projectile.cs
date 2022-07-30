using System;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [Header("Projectile Configuration")]
    [SerializeField] protected float projectileSpeed = 0.0f;
    [SerializeField] protected int projectileDamage = 0;
    protected Vector2 velocity;

    [Header("Screen Bound Parameters")]
    protected Vector2 spriteSize;
    protected ScreenBoundaries bounds;

    private Action<Projectile> returnToPool;

    private void Start()
    {
        bounds = ScreenBoundaries.Instance;
        spriteSize = GetComponent<SpriteRenderer>().bounds.extents;
    }

    void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        returnToPool.Invoke(this);

        /*if (collision.CompareTag("Player"))*/
    }

    protected virtual void Move()
    {
        transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;

        if (bounds.DistanceFromBounds(transform.position) > spriteSize.y)
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

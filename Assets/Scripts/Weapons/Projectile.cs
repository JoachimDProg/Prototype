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

    protected Action<Projectile> returnToPool;

    private void Awake()
    {
        bounds = ScreenBoundaries.Instance;
        spriteSize = GetComponent<SpriteRenderer>().bounds.extents;
    }

    private void Start()
    {

    }

    void Update()
    {
        Move();
        ReturnToPoolWhenOutOfBounds();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        returnToPool.Invoke(this);

        /*if (collision.CompareTag("Player"))*/
    }

    private void ReturnToPoolWhenOutOfBounds()
    {
        if (bounds.DistanceFromBounds(transform.position) > spriteSize.y)
        {
            returnToPool.Invoke(this);
        }
    }

    protected virtual void Move()
    {
        transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;
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

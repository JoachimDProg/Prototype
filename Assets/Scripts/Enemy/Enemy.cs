using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Configuration")]
    [SerializeField] protected float shootPermissionTimer;
    private Action<Enemy> returnToBase;
    protected IMovement movement;
    protected Gun gun;
    protected float movementSpeed = 0f;
    protected float canShootTimer = 0f;
    protected bool canShoot = false; // for delaying shoot when entering bounds
    protected Vector3 initialUp;

    [Header("Sine Configuration")]
    protected Dictionary<string, float> sineParam = default;
    protected bool isSine = false;

    [Header("Screen Bound Parameters")]
    protected bool hasEnteredBounds = false;
    protected bool isInsideBounds = false;
    protected ScreenBoundaries screenBoundaries;
    protected Vector2 spriteSize;

    [Header("Object References")]
    protected GameObject player;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        gun = GetComponentInChildren<Gun>();
        spriteSize = GetComponent<SpriteRenderer>().bounds.size;
        screenBoundaries = ScreenBoundaries.Instance;
        player = GameObject.FindGameObjectWithTag("Player"); // change to playerManager
        canShootTimer = shootPermissionTimer;
        initialUp = transform.up;

        if (isSine)
        {
            SineMove sineMove = new SineMove();
            sineMove = movement as SineMove;
            sineMove.sineParam = sineParam;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        UpdateBoundStatus(transform.position);
        UpdateShootPermissionTimer();
        Shoot();
        CanReturnToBase();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player Projectile")
        {
            returnToBase.Invoke(this);
        }
    }

    public virtual void Move()
    {
        transform.position += movement.Move(transform.position, initialUp, transform.up, movementSpeed);
    }
    protected void UpdateBoundStatus(Vector3 position)
    {
        if (screenBoundaries.IsInsideBounds(position))
            hasEnteredBounds = true;

        isInsideBounds = screenBoundaries.IsInsideBounds(position);
    }

    protected void UpdateShootPermissionTimer()
    {
        if (!canShoot)
        {
            canShootTimer -= Time.deltaTime;

            if (canShootTimer <= 0)
            {
                canShootTimer = shootPermissionTimer;
                canShoot = true;
            }
        }
    }

    protected abstract void Shoot();

    private void CanReturnToBase()
    {
        if (hasEnteredBounds && screenBoundaries.DistanceFromBounds(transform.position) > spriteSize.y)
            returnToBase.Invoke(this);
    }

    public void InitParameters(Vector2 position, Vector2 direction, float speed, Action<Enemy> returnToBase)
    {
        transform.position = position;
        Vector3 eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, direction));
        transform.eulerAngles = eulerAngles;
        movementSpeed = speed;

        this.returnToBase = returnToBase;
    }

    public void InitMove(IMovement enemyMovement, bool isSine, Dictionary<string, float> sineParam)
    {
        this.movement = enemyMovement;
        this.isSine = isSine;
        this.sineParam = sineParam;
    }
}
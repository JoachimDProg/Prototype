using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Variables")]
    private Action<Enemy> returnToBase;
    protected IMovement movement;
    protected float movementSpeed;
    protected Vector3 initialUp;

    [Header("Sine Configuration")]
    protected Dictionary<string, float> sineParam;
    protected bool isSine = false;

    [Header("Screen Bound Parameters")]
    protected ScreenBoundaries screenBoundaries;
    protected bool hasEnteredBounds = false;
    protected bool isInsideBounds = false;
    protected Vector2 spriteSize;

    [Header("Object References")]
    protected GameManager gameManager;
    protected Player player;

    protected void Awake()
    {
        screenBoundaries = ScreenBoundaries.Instance;
        spriteSize = GetComponent<SpriteRenderer>().bounds.size;

        gameManager = GameManager.Instance;
        player = gameManager.player;
    }

    protected virtual void Start()
    {
        initialUp = transform.up;

        if (isSine)
        {
            SineMove sineMove = new SineMove();
            sineMove = movement as SineMove;
            sineMove.sineParam = sineParam;
        }
    }

    protected virtual void Update()
    {
        Move();
        UpdateBoundStatus(transform.position);
        CanReturnToBase();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player Projectile")
        {
            returnToBase.Invoke(this);
        }
    }

    private void CanReturnToBase()
    {
        if (hasEnteredBounds && screenBoundaries.DistanceFromBounds(transform.position) > spriteSize.y)
            returnToBase.Invoke(this);
    }

    protected void UpdateBoundStatus(Vector3 position)
    {
        if (screenBoundaries.IsInsideBounds(position))
            hasEnteredBounds = true;

        isInsideBounds = screenBoundaries.IsInsideBounds(position);
    }

    public virtual void Move()
    {
        transform.position += movement.Move(transform.position, initialUp, transform.up, movementSpeed);
    }

    public void InitParameters(Vector2 position, Vector2 direction, float speed, Action<Enemy> returnToBase)
    {
        transform.position = position;
        Vector3 eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, direction));
        transform.eulerAngles = eulerAngles;
        movementSpeed = speed;

        this.returnToBase = returnToBase;
    }

    public void InitMove(IMovement movement, bool isSine, Dictionary<string, float> sineParam)
    {
        this.movement = movement;
        this.isSine = isSine;
        this.sineParam = sineParam;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Configuration")]
    [SerializeField] protected float shootPermissionTimer;
    protected Gun gun;
    private Action<Enemy> returnToBase;
    protected float movementSpeed;
    protected float canShootTimer;
    protected bool canShoot = false; // for delaying shoot when entering bounds

    [Header("Sine Configuration")]
    protected bool isSine;
    protected bool inverted;
    protected float amplitude;
    protected float frequency;
    protected float offset;
    protected float sin = 0;

    [Header("Screen Bound Parameters")]
    [SerializeField] protected bool hasEnteredBounds = false;
    [SerializeField] protected bool isInsideBounds;
    protected ScreenBoundaries bounds;
    protected Vector2 spriteSize;

    [Header("Object References")]
    [SerializeField] protected GameObject player;

    // Start is called before the first frame update
     protected virtual void Start()
    {
        gun = GetComponentInChildren<Gun>();
        spriteSize = GetComponent<SpriteRenderer>().bounds.size;
        bounds = ScreenBoundaries.Instance;
        player = GameObject.FindGameObjectWithTag("Player"); // change to playerManager
        canShootTimer = shootPermissionTimer;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        UpdateBoundStatus(transform.position);
        UpdateShootPermissionTimer();
        Shoot();
    }

    private void Move()
    {
        Vector3 position = transform.position;

        if (isSine)
            sin = Sine(position);

        float posX = movementSpeed * Time.deltaTime * sin;
        float posY = movementSpeed * Time.deltaTime;

        transform.position += new Vector3(posX, -posY);

        if (hasEnteredBounds && bounds.DistanceFromBounds(transform.position) > spriteSize.y)
            returnToBase.Invoke(this);
    }

    private float Sine(Vector3 position)
    {
        float sin = Mathf.Sin(position.y * frequency + (offset * Mathf.PI)) * amplitude;

        if (inverted)
            sin *= -1;

        return sin;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        returnToBase.Invoke(this);
    }

    protected abstract void Shoot();

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

    protected void UpdateBoundStatus(Vector3 position)
    {
        if (bounds.IsInsideBounds(position))
            hasEnteredBounds = true;

        isInsideBounds = bounds.IsInsideBounds(position);
    }

    public void InitParameters(Vector2 position, Vector2 direction, float speed, Action<Enemy> returnToBase)
    {
        transform.position = position;
        Vector3 eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, direction));
        transform.eulerAngles = eulerAngles;
        movementSpeed = speed;

        this.returnToBase = returnToBase;
    }

    public void InitSine(bool isSine, bool inverted, float amplitude, float frequency, float offset)
    {
        this.isSine = isSine;
        this.inverted = inverted;
        this.amplitude = amplitude;
        this.frequency = frequency;
        this.offset = offset;
    }
}

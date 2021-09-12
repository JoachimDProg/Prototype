using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Configuration")]
    protected float movementSpeed;
    [SerializeField] protected Gun gun;
    private Action<Enemy> returnToBase;

    [Header("Sine Configuration")]
    protected bool sine;
    protected bool inverted;
    protected float amplitude;
    protected float frequency;
    protected float offset;
    protected float sin = 0;

    [Header("Screen Bound Parameters")]
    [SerializeField] protected Vector2 spriteSize;
    protected ScreenBoundaries bounds;
    [SerializeField] protected bool hasEnteredScreen = false;

    // Start is called before the first frame update
    void Start()
    {
        gun = GetComponentInChildren<Gun>();
        spriteSize = GetComponent<SpriteRenderer>().bounds.size;
        bounds = ScreenBoundaries.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (bounds.IsInsideBounds(transform.position))
            hasEnteredScreen = true;
        //Shoot();
    }

    private void Move()
    {
        Vector3 position = transform.position;

        if (sine)
            sin = Sine(position);

        float posX = movementSpeed * Time.deltaTime * sin;
        float posY = movementSpeed * Time.deltaTime;

        transform.position += new Vector3(posX, -posY);

        if (hasEnteredScreen && bounds.DistanceFromBounds(transform.position) > spriteSize.y)
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

    private void Shoot()
    {
        gun.Shoot();
    }

    public void InitParameters(Vector2 position, Vector2 direction, float speed, Action<Enemy> returnToBase)
    {
        transform.position = position;
        Vector3 eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, direction));
        transform.eulerAngles = eulerAngles;
        movementSpeed = speed;

        this.returnToBase = returnToBase;
    }

    public void InitSine(bool sine, bool inverted, float amplitude, float frequency, float offset)
    {
        this.sine = sine;
        this.inverted = inverted;
        this.amplitude = amplitude;
        this.frequency = frequency;
        this.offset = offset;
    }
}

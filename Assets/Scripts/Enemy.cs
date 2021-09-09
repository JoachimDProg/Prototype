using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Configuration")]
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected Gun gun;
    private Action<Enemy> returnToBase;

    [Header("Wave Configuration")]
    [SerializeField] protected float amplitude;
    [SerializeField] protected float frequency;
    [SerializeField] protected float offset;
    [SerializeField] protected bool inverted;
    protected Vector2 parentUp;

    [Header("Bound Parameters")]
    [SerializeField] protected float padding;
    [SerializeField] protected Vector2 spriteSize;
    protected bool hasEnteredScreen = false;
    // protected bool HasEnteredScreen => ScreenBoundaries.Instance.screenBounds.Contains(transform.position);

    // Start is called before the first frame update
    void Start()
    {
        gun = GetComponentInChildren<Gun>();
        spriteSize = GetComponent<SpriteRenderer>().bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        MoveSine();
        if (ScreenBoundaries.Instance.screenBounds.Contains(transform.position))
            hasEnteredScreen = true;
        //Shoot();
    }

    private bool HasEnteredScreen(Vector2 position)
    {
        return ScreenBoundaries.Instance.screenBounds.Contains(position);
    }

    private void MoveSine()
    {
        Vector3 position = transform.position;

        float sin = Mathf.Sin(position.y * frequency + (offset * Mathf.PI)) * amplitude;
        if (inverted) sin *= -1;

        float posX = movementSpeed * Time.deltaTime * sin;
        float posY = movementSpeed * Time.deltaTime;

        transform.position += new Vector3(posX, -posY);

        if (ScreenBoundaries.Instance.DistanceFromBounds(transform.position) > spriteSize.y && hasEnteredScreen)
        {
            returnToBase.Invoke(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        returnToBase.Invoke(this);
    }

    private void Shoot()
    {
        gun.Shoot();
    }

    public void InitParameters(Vector2 position, Vector2 direction, Action<Enemy> returnToBase)
    {
        transform.position = position;
        // mesure l'angle de rotation z entre le world up et le up du parent
        Vector3 eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, direction));
        transform.eulerAngles = eulerAngles;
        parentUp = direction;

        this.returnToBase = returnToBase;
    }
}

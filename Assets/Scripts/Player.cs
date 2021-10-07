using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Configuration")]
    [SerializeField] private float playerMoveSpeedNormal = 0f;
    [SerializeField] private float playerMoveSpeedSlow = 0f;
    [SerializeField] private float speedBoost = 0f;
    private float playerMoveSpeed = 0f;
    private Gun[] guns;
    private bool gunBoost = false;
    private Shield shield;
    
    [Header("Screen Bound Parameters")]
    private Vector2 spriteSize;
    protected ScreenBoundaries stageLimits;

    void Start()
    {
        guns = GetComponentsInChildren<Gun>(true);
        shield = GetComponentInChildren<Shield>(true);
        spriteSize = GetComponent<SpriteRenderer>().bounds.extents;
        stageLimits = ScreenBoundaries.Instance;
    }

    void Update()
    {
        Move();
        Shoot();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            playerMoveSpeed = playerMoveSpeedSlow;
        else
            playerMoveSpeed = playerMoveSpeedNormal;

        float xInput = Input.GetAxis("Horizontal") * playerMoveSpeed * Time.deltaTime;
        float yInput = Input.GetAxis("Vertical") * playerMoveSpeed * Time.deltaTime;
        Vector3 input = new Vector2(xInput, yInput);

        transform.position = stageLimits.ClampPlayerPosition(transform.position + input, spriteSize);
    }

    private void Shoot()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!gunBoost)
            {
                guns[0].Shoot();
            }
            else
            {
                foreach (Gun gun in guns)
                {
                    gun.Shoot();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Power Up")
        {
            PowerUp powerUp = collision.GetComponent<PowerUp>();
            if (powerUp.guns)
                gunBoost = true;
            if (powerUp.shield)
                shield.gameObject.SetActive(true);
            if (powerUp.speed)
                playerMoveSpeedNormal += speedBoost;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            gameObject.SetActive(false);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool isInvincible = false;
    private float invincibilityTimer = 1f;
    
    [Header("Screen Bound Parameters")]
    private Vector2 shipBounds;
    private Vector2 shipSize;
    private Vector2 shieldSize;
    protected ScreenBoundaries screenBoundaries;

    void Start()
    {
        guns = GetComponentsInChildren<Gun>(true);
        shield = GetComponentInChildren<Shield>(true);
        shieldSize = shield.GetComponent<SpriteRenderer>().bounds.extents;
        shipSize = GetComponent<SpriteRenderer>().bounds.extents;
        screenBoundaries = ScreenBoundaries.Instance;
    }

    void Update()
    {
        Move();
        Shoot();
        UpdateInvincibilityTimer();
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

        shipBounds = shield.isActiveAndEnabled ? shieldSize : shipSize;
        transform.position = screenBoundaries.ClampPlayerPosition(transform.position + input, shipBounds);
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
            {
                shield.gameObject.SetActive(true);
                invincibilityTimer = 1;
            }

            if (powerUp.speed)
                playerMoveSpeedNormal += speedBoost;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            if (shield.gameObject.activeInHierarchy)
            {
                shield.gameObject.SetActive(false);
                isInvincible = true;
            }
            else if (!isInvincible)
            {
                gameObject.SetActive(false);
                SceneManager.LoadScene("Game Over Menu");
            }
                
        }
    }

    private void UpdateInvincibilityTimer()
    {
        if (isInvincible)
            invincibilityTimer -= Time.deltaTime;

        if (invincibilityTimer <= 0)
        {
            invincibilityTimer = 1;
            isInvincible = false;
        }
    }
}

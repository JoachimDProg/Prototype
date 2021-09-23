using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Configuration")]
    [SerializeField] private float playerMoveSpeedNormal;
    [SerializeField] private float playerMoveSpeedSlow;
    private float playerMoveSpeed;
    private Gun[] guns;
    
    [Header("Screen Bound Parameters")]
    private Vector2 spriteSize;
    protected ScreenBoundaries stageLimits;

    void Start()
    {
        guns = GetComponentsInChildren<Gun>();
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
            foreach(Gun gun in guns)
            {
                gun.Shoot();
            }
        }
    }
}

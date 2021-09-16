using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Configuration")]
    private float playerMoveSpeed;
    [SerializeField] private float playerMoveSpeedNormal;
    [SerializeField] private float playerMoveSpeedSlow;
    [SerializeField] private Gun[] guns;
    private Vector2 playerSpriteSize;

    void Start()
    {
        guns = GetComponentsInChildren<Gun>();
        playerSpriteSize = GetComponent<SpriteRenderer>().bounds.extents;
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

        transform.position = ScreenBoundaries.Instance.ClampPlayerPosition(transform.position + input, playerSpriteSize);
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

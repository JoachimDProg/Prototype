using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Configuration")]
    [SerializeField] private float playerMoveSpeedNormal = 20.0f;
    [SerializeField] private float playerMoveSpeedSlow = 12.0f;
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
        // get Axis and multiply by move speed and time
        float xInput = Input.GetAxis("Horizontal") * playerMoveSpeedNormal * Time.deltaTime;
        float yInput = Input.GetAxis("Vertical") * playerMoveSpeedNormal * Time.deltaTime;
        Vector3 input = new Vector2(xInput, yInput);

        // new position equals old position + input
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

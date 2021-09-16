using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float shootAngle;
    [SerializeField] private float seekCooldown;
    private float canSeekTimer;
    private bool canSeek = true;

    protected override void Start()
    {
        base.Start();
        canSeekTimer = seekCooldown;
    }

    protected override void Shoot()
    {
        if (canSeek)
        {
            Vector3 currentPos = transform.position;
            Vector3 playerPos = player.transform.position;

            float angleToTarget = Math.GetAngle(transform.up, Math.GetVector(currentPos, playerPos));
            float angleToRotate = angleToTarget * Time.deltaTime * rotationSpeed;
            float lookAngle = angleToTarget * Mathf.Rad2Deg;
            float rotateSign = Mathf.Sign(angleToTarget);

            Debug.Log("Angle to rotate: " + angleToRotate);
            Debug.Log("Look Angle: " + lookAngle);

            if (angleToRotate >= -1 && angleToRotate < 0)
                angleToRotate = -1f;
            else if (angleToRotate > 0 && angleToRotate <= 1)
                angleToRotate = 1f;

            Vector3 eulerAngles = new Vector3(0, 0, angleToRotate);

            transform.Rotate(eulerAngles);

            if (isInsideBounds && canShoot && lookAngle <= shootAngle)
            {
                gun.Shoot();
                canSeek = false;
            }
        }
        else if (!canSeek)
        {
            canSeekTimer -= Time.deltaTime;

            if (canSeekTimer <= 0)
            {
                canSeek = true;
                canSeekTimer = seekCooldown;
            }   
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] [Range(0.0f, 1.0f)] private float minRotation;
    [SerializeField] [Range(1, 10)] private int shootAngle;
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

            // calculate angle to target in RAD
            float angleToTarget = Math.GetAngle(transform.up, Math.GetVector(currentPos, playerPos));
            // calculate angle to rotate this frame in RAD
            float angleToRotate = angleToTarget * Time.deltaTime * rotationSpeed;

            // check if rotation is positive or negative
            float rotateSign = Mathf.Sign(angleToTarget);
            // calculate angle to rotate this frame in DEG
            float lookAngle = angleToTarget * Mathf.Rad2Deg * rotateSign;

            Debug.Log("Look Angle: " + lookAngle);

            // if rotation for this frame is too small, increase rotation
            if (angleToRotate >= -minRotation && angleToRotate < 0)
                angleToRotate = -minRotation;
            else if (angleToRotate > 0 && angleToRotate <= minRotation)
                angleToRotate = minRotation;

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

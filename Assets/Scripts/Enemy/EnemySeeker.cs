using UnityEngine;

public class EnemySeeker : Enemy, IShoot
{
    [SerializeField][Range(0.0f, 50.0f)] private float rotationSpeed = default;
    [SerializeField][Range(0.1f, 3.0f)] private float minRotation = default;
    [SerializeField][Range(1, 10)] private int shootAngle = default;
    [SerializeField] private float seekCooldown = default;
    private float canSeekTimer = default;
    private bool canShoot = false; // for delaying shoot when entering bounds
    private bool canSeek = true;

    [SerializeField] private float shootCooldown = default;
    private float canShootTimer = default;
    private Gun gun;

    protected override void Start()
    {
        base.Start();

        gun = GetComponentInChildren<Gun>();
        canShootTimer = shootCooldown;
        canSeekTimer = seekCooldown;
    }

    protected override void Update()
    {
        base.Update();

        UpdateShootPermissionTimer();
        Shoot();
    }

    private void UpdateShootPermissionTimer()
    {
        if (!canShoot)
        {
            canShootTimer -= Time.deltaTime;

            if (canShootTimer <= 0)
            {
                canShootTimer = shootCooldown;
                canShoot = true;
            }
        }
    }

    public void Shoot()
    {
        if (isInsideBounds && canSeek)
        {
            Vector3 currentPos = transform.position;
            Vector3 playerPos = player.transform.position;

            // calculate angle to target in RAD
            float angleToTarget = Math.GetAngle(transform.up, Math.GetVector(currentPos, playerPos));
            // calculate angle to rotate this frame in RAD (for rotation)
            float angleToRotate = angleToTarget * Time.deltaTime * rotationSpeed;


            // check if rotation is positive or negative
            float rotateSign = Mathf.Sign(angleToTarget);
            // calculate angle to rotate this frame in (for shooting permission)
            float lookAngle = angleToTarget * Mathf.Rad2Deg * rotateSign;


            // if rotation for this frame is too small, increase rotation
            if (Mathf.Abs(angleToRotate) < minRotation)
                angleToRotate = minRotation * rotateSign;

            Vector3 eulerAngles = new Vector3(0, 0, angleToRotate);

            transform.Rotate(eulerAngles);

            if (canShoot && lookAngle <= shootAngle)
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

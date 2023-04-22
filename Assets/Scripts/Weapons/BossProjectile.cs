using UnityEngine;

public class BossProjectile : Projectile
{
    [Header("Boss Projectile Parameters")]
    [SerializeField] private float curveX = 0.1f;
    [SerializeField] private float curveY = 0.1f;
    [SerializeField] private float curveModifier = 0.1f;
    private float curveXinit = 0.1f;
    private float curveYinit = 0.1f;
    private float curveModifierInit = 0.5f;
    protected Vector3 initialUp;
    protected Vector3 initialRight;

    private void Start()
    {
        initialUp = transform.up;
        initialRight = transform.right;
        curveXinit = curveX;
        curveYinit = curveY;
        curveModifierInit = curveModifier;

        Debug.Log($"local rotation z: {transform.localRotation.z}");
        Debug.Log($"world rotation z: {transform.rotation.z}");
        Debug.Log($"local Euler Angles: {transform.localEulerAngles}");
    }

    protected override void Move()
    {
        // curvy effect
        transform.localEulerAngles += new Vector3(0, 0, curveModifier);
        direction = new Vector2(transform.up.x, transform.up.y);

        velocity = direction * projectileSpeed;
        transform.localPosition += new Vector3(velocity.x, velocity.y) * Time.deltaTime;



        // cool effect 1
        // direction = new Vector2(transform.up.x + curveX, transform.up.y + curveY);

        // death trap: speed 5, curve 0.5 
        // direction = new Vector2(transform.up.x, transform.up.y);
    }

    private void OnDisable()
    {
        curveX = curveXinit;
        curveY = curveYinit;
        curveModifier = curveModifierInit;
    }
}

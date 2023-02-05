using UnityEngine;

public class BossProjectile : Projectile
{
    [SerializeField] private float curveX = 1.0f;
    [SerializeField] private float curveY = 1.0f;
    [SerializeField] private float curveModifier = 1.0f;
    private float curveXinit = 1.0f;
    private float curveYinit = 1.0f;
    private float curveModifierInit = 1.0f;
    protected Vector3 initialUp;
    protected Vector3 initialRight;

    private void Start()
    {
        initialUp = transform.up;
        initialRight = transform.right;
        curveXinit = curveX;
        curveYinit = curveY;
        curveModifierInit = curveModifier;

        /*Debug.Log("Velocity: " + velocity);
        Debug.Log("InitUp: " + initialUp);
        Debug.Log("InitRight: " + initialRight);
        Debug.Log("spriteSize: " + spriteSize);
        Debug.Log("curveXINIT: " + curveX);
        Debug.Log("curveYINIT: " + curveY);*/
    }

    protected override void Move()
    {
        // curvy effect
        transform.eulerAngles += new Vector3(0, 0, curveModifier);
        direction = new Vector2(transform.up.x, transform.up.y);

        velocity = direction * projectileSpeed;
        transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;



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

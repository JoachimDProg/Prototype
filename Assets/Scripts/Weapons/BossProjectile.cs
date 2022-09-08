using UnityEngine;

public class BossProjectile : Projectile
{
    [SerializeField] private float curveX = 0.0f;
    [SerializeField] private float curveY = 0.0f;
    protected Vector3 initialUp;

    private void Start()
    {
        initialUp = transform.up;
    }

    private float time = 0.0f;
    protected override void Move()
    {
        Debug.Log(this.name + this.gameObject.activeInHierarchy);

        time += Time.deltaTime;
        if (time > 0.01)
        {
            curveX++;
            curveY--;

            time = 0.0f;
        }

        if (!bounds.IsInsideBounds(this.transform.position))
        {
            curveX = 1;
            curveY = 1;
        }

        transform.position += new Vector3(velocity.x * initialUp.x - curveX,
                                          velocity.y * initialUp.y + curveY)
                                          * Time.deltaTime;
    }
}

using UnityEngine;

public class BossProjectile : Projectile
{
    [SerializeField] private float curveX = 0.0f;
    [SerializeField] private float curveY = 0.0f;

    private float time = 0.0f;
    protected override void Move()
    {
        time += Time.deltaTime;
        if(time > 0.01)
        {
            curveX++;
            curveY--;

            time = 0.0f;
        }
        
        transform.position += new Vector3(velocity.x + curveX, velocity.y + curveY) * Time.deltaTime;
    }
}

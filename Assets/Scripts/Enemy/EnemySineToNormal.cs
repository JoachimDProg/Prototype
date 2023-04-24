public class EnemySineToNormal : Enemy
{
    public override void Move()
    {
        base.Move();

        if (transform.position.y < player.transform.position.y)
            movement = new NormalMove();
    }
}

public class BossGun : Gun
{
    protected override void OnShoot()
    {
        // TODO change to bullet pattern
        // TODO create diverse bullet pattern
        Projectile bossProjectile = projectileMagazine.Dequeue();
        bossProjectile.gameObject.SetActive(true);
        bossProjectile.InitParameters(transform.position, transform.up, RefillMagazine);

        /*if (audioSource.isActiveAndEnabled)
            audioSource.Play();*/
    }
}

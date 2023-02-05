public class BossGun : Gun
{
    protected override void OnShoot()
    {
        Projectile bossProjectile = projectileMagazine.Dequeue();
        bossProjectile.gameObject.SetActive(true);
        bossProjectile.InitParameters(transform.position, transform.up, RefillMagazine);

        if (audioSource.isActiveAndEnabled)
            audioSource.Play();
    }
}

public class BossGun : Gun
{
    protected override void OnShoot()
    {
        // TODO change to bullet pattern
        // TODO create diverse bullet pattern
        Projectile laser = projectileMagazine.Dequeue();
        laser.gameObject.SetActive(true);
        laser.InitParameters(transform.position, transform.up, RefillMagazine);

        /*if (audioSource.isActiveAndEnabled)
            audioSource.Play();*/
    }
}

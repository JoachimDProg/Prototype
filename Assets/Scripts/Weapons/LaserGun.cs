public class LaserGun : Gun
{
    protected override void OnShoot()
    {
        Projectile laser = projectileMagazine.Dequeue();
        laser.gameObject.SetActive(true);
        laser.InitParameters(transform.position, transform.up, RefillMagazine);

        if(audioSource.isActiveAndEnabled)
            audioSource.Play();
    }
}

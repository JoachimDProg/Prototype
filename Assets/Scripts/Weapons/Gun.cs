using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [Header("Gun Configuration")]
    [SerializeField] protected Projectile projectilePrefab;
    [SerializeField] protected int magazineCapacity;
    [SerializeField] protected float gunShootRate;
    protected AudioSource audioSource;

    [Header("Gun Parameter")]
    protected float gunCooldownTimer = 0.0f;
    protected Queue<Projectile> projectileMagazine;
    protected bool CanShoot => gunCooldownTimer <= 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        FillMagazine();
    }

    void Update()
    {
        UpdateShootCooldown();
    }

    protected void FillMagazine()
    {
        projectileMagazine = new Queue<Projectile>();

        for (int i = 0; i < magazineCapacity; i++)
        {
            Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.gameObject.SetActive(false);
            projectileMagazine.Enqueue(projectile);
        }
    }

    private void UpdateShootCooldown()
    {
        gunCooldownTimer -= Time.deltaTime;
        if (gunCooldownTimer < 0)
            gunCooldownTimer = 0;
    }

    protected void RefillMagazine(Projectile projectile)
    {
        projectileMagazine.Enqueue(projectile);
        projectile.gameObject.SetActive(false);
    }

    protected abstract void OnShoot();

    public void Shoot()
    {
        if (CanShoot)
        {
            OnShoot();
            gunCooldownTimer = gunShootRate;
        }
    }
}

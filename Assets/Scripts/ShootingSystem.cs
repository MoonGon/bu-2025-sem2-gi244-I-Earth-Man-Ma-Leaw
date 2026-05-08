using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float bulletLifeTime = 2f;

    [Header("Shooting Logic")]
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            PlayerController player = GetComponent<PlayerController>();
            float playerSpeed = (player != null) ? player.currentSpeed : 0;

            rb.linearVelocity = transform.forward * (bulletSpeed + playerSpeed);
        }

        Destroy(bullet, bulletLifeTime);
    }
}
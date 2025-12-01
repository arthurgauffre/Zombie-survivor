using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Shoot Settings")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootForce = 20f;
    public float shootRate = 0.5f;

    private float nextShootTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1"))
            TryShoot();
    }

    public void TryShoot()
    {
        if (Time.time >= nextShootTime)
        {
            ShootBullet();
            nextShootTime = Time.time + shootRate;
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
        }
    }
}
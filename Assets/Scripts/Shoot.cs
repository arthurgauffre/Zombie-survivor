using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Shoot Settings")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootForce = 20f;
    public float shootRate = 0.5f;

    [Header("Reload Settings")]
    public int maxAmmo = 30;
    public float reloadTime = 1f;

    private int currentAmmo;
    private bool isReloading = false;

    private float reloadTimer = 0f;
    private float nextShootTime = 0f;

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= reloadTime)
            {
                currentAmmo = maxAmmo;
                isReloading = false;
                reloadTimer = 0f;
            }
            return;
        }

        if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1"))
            TryShoot();

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            isReloading = true;
            animator.SetTrigger("Reload");
            reloadTimer = 0f;
        }
    }

    public void TryShoot()
    {
        if (Time.time >= nextShootTime)
        {
            animator.ResetTrigger("Shoot");
            ShootBullet();
            animator.SetTrigger("Shoot");
            nextShootTime = Time.time + shootRate;

            currentAmmo--;
            if (currentAmmo <= 0)
            {
                isReloading = true;
                animator.SetTrigger("Reload");
                reloadTimer = 0f;
            }
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
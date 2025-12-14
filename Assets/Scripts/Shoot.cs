using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Shoot Settings")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootForce = 20f;
    public float shootRate = 0.5f;

    public AudioClip shootSound;
    public AudioClip reloadSound;

    [Header("Reload Settings")]
    public int maxAmmo = 30;
    public float reloadTime = 1f;

    private int currentAmmo;
    private bool isReloading = false;

    private float reloadTimer = 0f;
    private float nextShootTime = 0f;

    private Animator animator;
    private AmmoHud ammoHud;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        currentAmmo = maxAmmo;
        ammoHud = FindObjectOfType<AmmoHud>();
        UpdateAmmoDisplay();
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
                UpdateAmmoDisplay();
            }
            return;
        }

        if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1"))
            TryShoot();

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            isReloading = true;
            animator.SetTrigger("Reload");
            AudioSource.PlayClipAtPoint(reloadSound, shootPoint.position);
            reloadTimer = 0f;
        }
    }

    public void TryShoot()
    {
        if (Time.time >= nextShootTime)
        {
            ShootBullet();
            nextShootTime = Time.time + shootRate;

            currentAmmo--;
            if (currentAmmo <= 0)
            {
                isReloading = true;
                animator.SetTrigger("Reload");
                AudioSource.PlayClipAtPoint(reloadSound, shootPoint.position);
                reloadTimer = 0f;
            }
            UpdateAmmoDisplay();
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        AudioSource.PlayClipAtPoint(shootSound, shootPoint.position);
        if (rb != null)
        {
            rb.AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
        }
    }

    private void UpdateAmmoDisplay()
    {
        if (ammoHud != null)
        {
            // ensure non-negative display
            int displayAmmo = Mathf.Max(0, currentAmmo);
            ammoHud.UpdateAmmo(displayAmmo, maxAmmo);
        }
    }
}
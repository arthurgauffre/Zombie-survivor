using UnityEngine;

public class Aim : MonoBehaviour
{
    private Animator animator;

    public Transform weaponTransform;

    public Transform cameraTransform;

    public Vector3 weaponRotationOffset = Vector3.zero;

    public float rotationLerpSpeed = 20f;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        bool isAiming = Input.GetButton("Fire2");
        animator.SetBool("Aiming", isAiming);

        AlignWeaponWithCamera();
    }

    private void AlignWeaponWithCamera()
    {
        if (weaponTransform == null || cameraTransform == null)
            return;

        Quaternion targetRotation = cameraTransform.rotation * Quaternion.Euler(weaponRotationOffset);

        float t = 1f - Mathf.Exp(-rotationLerpSpeed * Time.deltaTime);
        weaponTransform.rotation = Quaternion.Slerp(weaponTransform.rotation, targetRotation, t);
    }
}

using UnityEngine;

public class Aim : MonoBehaviour
{
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isAiming = Input.GetButton("Fire2");
        animator.SetBool("Aiming", isAiming);

        if (isAiming)
            Debug.Log("Aiming");
    }
}

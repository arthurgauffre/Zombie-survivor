using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Health targetHealth = collision.gameObject.GetComponent<Health>();
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Debug.Log("Hit a zombie!");
        }
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}

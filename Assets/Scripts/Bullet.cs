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
        Zombies zombie = collision.collider.GetComponent<Zombies>();

        if (zombie != null)
        {
            zombie.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}

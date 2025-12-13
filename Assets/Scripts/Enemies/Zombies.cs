using UnityEngine;
using UnityEngine.AI;

public class Zombies : MonoBehaviour
{
    public int health = 3;
    public float attackRange = 1.5f;

    private bool isDead = false;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        agent.SetDestination(player.position);

        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            animator.SetTrigger("Bite");
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        Destroy(gameObject, 4f);
    }
}

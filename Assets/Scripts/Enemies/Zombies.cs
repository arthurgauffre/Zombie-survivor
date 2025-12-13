using UnityEngine;
using UnityEngine.AI;

public class Zombies : MonoBehaviour
{
    public int health = 3;
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);

            if (distance <= attackRange)
            {
                animator.SetTrigger("attack");
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            agent.isStopped = true;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        animator.SetTrigger("hit");

        if (health <= 0)
        {
            Die();
        }
        else if (health == 2)
        {
            animator.SetTrigger("jump");
        }
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("die");
        Destroy(gameObject, 3f);
    }
}

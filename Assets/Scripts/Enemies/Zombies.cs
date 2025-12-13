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

        DeactivateRagdoll();
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
        agent.enabled = false;
        animator.SetTrigger("Die");

        ActivateRagdoll(); 
        Destroy(gameObject, 5f);
    }

    public void DeactivateRagdoll()
    {
        animator.enabled = true;
        foreach (Rigidbody bone in
        GetComponentsInChildren<Rigidbody>())
        {
            bone.isKinematic = true;
            bone.detectCollisions = false;
        }
        foreach (CharacterJoint joint in
        GetComponentsInChildren<CharacterJoint>())
        {
            joint.enableProjection = true;
        }
        foreach (Collider col in
        GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
    }

    public void ActivateRagdoll()
    {
        foreach (Rigidbody bone in
        GetComponentsInChildren<Rigidbody>())
        {
            bone.isKinematic = false;
            bone.detectCollisions = true;
        }
        foreach (Collider col in
        GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
    }
}

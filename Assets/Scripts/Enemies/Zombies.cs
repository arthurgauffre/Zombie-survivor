using UnityEngine;
using UnityEngine.AI;

public class Zombies : MonoBehaviour
{
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

   public void Die()
    {
        if (isDead) return;

        isDead = true;
        agent.isStopped = true;
        agent.enabled = false;
        animator.SetTrigger("Die");

        ActivateRagdoll(); 
        Destroy(gameObject, 5f);
        // update the score
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(100);
        }
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

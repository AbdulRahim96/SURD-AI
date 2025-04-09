using UnityEngine;
using UnityEngine.AI;

public class Enemy : Health
{
    private NavMeshAgent agent;
    private Animator animator;
    private Collider collider;
    public ParticleSystem shootingParticle;
    public float attackRange = 50;
    public float distance;
    [SerializeField]  private Transform player;
    private bool isDead;
    private bool isAttacking;
    private void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        agent.stoppingDistance = attackRange;
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            shootingParticle.Stop();
            return;
        }
        distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackRange + 0.2f)
        {
            animator.SetBool("chasing", true);
            agent.SetDestination(player.position);
            isAttacking = false;
        }
        else
        {
            Attack();
        }
        

    }

    private float Distance()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    private void Attack()
    {
        animator.SetBool("chasing", false);
        transform.LookAt(player);
        agent.SetDestination(transform.position);
        if (isAttacking) return;
        shootingParticle.Play();
        isAttacking = true;
    }

    public void Die()
    {
        isDead = true;
        animator.Play("Die");
        collider.enabled = false;
        tag = "Untagged";
        Destroy(gameObject, 3);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

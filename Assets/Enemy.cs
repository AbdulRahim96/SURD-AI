using System.Collections;
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
    public bool isAttacking;
    private void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        agent.stoppingDistance = attackRange;
    }

    public void Start()
    {
        if(isAttacking)
            StartCoroutine(Attack());
    }

    public void SetChase(bool check)
    {
        animator.SetBool("chasing", check);
    }


    public void Die()
    {
        StopAllCoroutines();
        isDead = true;
        animator.Play("Die");
        collider.enabled = false;
        tag = "Untagged";
        Destroy(gameObject, 3);
    }

    private IEnumerator moving()
    {
        while (Vector3.Distance(transform.position, player.position) >= agent.stoppingDistance)
        {
            animator.SetBool("chase", true);
            agent.SetDestination(player.position);
            yield return null;
        }

        agent.SetDestination(transform.position);
        animator.SetBool("chase", false);
    }

    IEnumerator Attack()
    {
        while (player)
        {
            // Wait until Move finishes
            yield return StartCoroutine(moving());

            // Then fire
            yield return StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        shootingParticle.Play();
        shootingParticle.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Agent : MonoBehaviour
{
    public string characterName, background;
    public ParticleSystem shootingParticle;
    public ParticleSystem machineGun;
    public ParticleSystem bazooka;
    public Transform target;
    public float attackDistance;
    public State currentState, requiredAction;
    public NavMeshAgent agent;
    public float moveSpeed;
    public Animator animator;
    public Transform rightHand;
    public Text verbalText;
    private float currentSpeed;
    public PlayerMovement playerMovement;
    public bool autoFire;
    public enum State
    {
        None,
        Moving,
        Attacking,
        pickup
    }

    private bool isRepeating;
    private PlayerMovement controller;
    public bool AI;
    private void Awake()
    {
        controller = GetComponent<PlayerMovement>();
    }

    public void PlayAction(string functionName) // testing through Button
    {
        StartCoroutine(functionName);
    }

    public void CallFunction(string functionName, Transform target = null)
    {
        StopAllCoroutines();
        shootingParticle.Stop();
        SetTarget(target);
        StartCoroutine(functionName);

        // InputManager.instance.AgentRespondText(functionName + " =>> " + target.name);
    }

    private void SetTarget(Transform t)
    {
        target = t;

        // Turn the target Object into red color
       // target.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    private void GetAllEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackDistance);
        foreach (Collider collider in colliders)
        {
            if(collider.gameObject.tag == "Enemy")
            {
                target = collider.gameObject.transform;
                return;
            }
        }
        target = null;
        autoFire = false;
        shootingParticle.Stop();
    }

    public void SetAI(bool ai)
    {
        AI = ai;
        controller.AI = AI;
    }

    void UpdateText(string str) // not in use at the moment
    {
        verbalText.text = "";
        verbalText.DOText(str, 1);
        verbalText.DOText("", 0).SetDelay(3);
    }

    #region All Actions
    IEnumerator Move()
    {
        while (Vector3.Distance(transform.position, target.position) >= agent.stoppingDistance)
        {
            currentSpeed = Mathf.Clamp01(agent.velocity.magnitude);
            animator.SetFloat("MoveZ", currentSpeed);
            agent.SetDestination(target.position);
            yield return null;
        }

        // reached
        // stop animation
        animator.SetFloat("MoveZ", 0);
    }
    IEnumerator Attack()
    {
        agent.stoppingDistance = attackDistance;
        while (target)
        {
            // Wait until Move finishes
            yield return StartCoroutine(Move());

            // Then fire
            yield return StartCoroutine(Fire());
        }
        shootingParticle.Stop();
        agent.Stop();
    }
    IEnumerator AttackAll()
    {
        autoFire = true;
        agent.stoppingDistance = attackDistance;

        while (target && target.CompareTag("Enemy"))
        {
            // Wait until Move finishes
            yield return StartCoroutine(Move());

            // Then fire
            yield return StartCoroutine(Fire());

            if (!target.CompareTag("Enemy"))
                GetAllEnemies();
        }
        autoFire = false;
        shootingParticle.Stop();
        agent.Stop();
    }
    IEnumerator Rocket_Launcher_Attack()
    {
        transform.DOLookAt(target.position, .5f);
        yield return StartCoroutine(Switch_Weapon());
        shootingParticle = bazooka;
        shootingParticle.Play();
    }
    IEnumerator Switch_Weapon()
    {
        yield return StartCoroutine(playerMovement.ChangeWeapon(1.9f / 2f));
    }
    IEnumerator Pick_up()
    {
        yield return StartCoroutine(Move());
        animator.SetTrigger("pickup");
        yield return new WaitForSeconds(4);
        target.SetParent(rightHand);
        target.localPosition = Vector3.zero;
    }
    private IEnumerator Fire()
    {
        print("firing");
        agent.SetDestination(transform.position);
        transform.LookAt(target);
        shootingParticle.Play();
        yield return new WaitForSeconds(1);
    }
    #endregion
}

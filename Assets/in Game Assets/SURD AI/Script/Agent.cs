using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Agent : MonoBehaviour
{
    public string characterName, background;
    public ParticleSystem shootingParticle;
    public Transform target;
    public State currentState, requiredAction;

    private NavMeshAgent agent;
    private Animator animator;

    public Text verbalText;

    public enum State
    {
        None,
        Moving,
        Attacking,
        pickup

    }

    private bool isRepeating;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void Apply(string functionName, Transform target = null)
    {
        SetTarget(target);
        Invoke(functionName, 0);

       // InputManager.instance.AgentRespondText(functionName + " =>> " + target.name);
    }

    private void Move()
    {
        print("moving");
        GetComponent<NavMeshAgent>().SetDestination(target.position);
        animator.SetBool("moving", true);
        animator.SetBool("attacking", false);
    }


    private void SetTarget(Transform t)
    {
        target = t;

        // Turn the target Object into red color
       // target.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Moving:
                if (Vector3.Distance(transform.position, target.position) < agent.stoppingDistance)
                {
                    if (requiredAction == State.Attacking)
                    {
                        currentState = State.Attacking;
                    }
                    else if (requiredAction == State.pickup)
                    {
                        animator.SetTrigger("pickup");
                        Idle();
                    }
                    else
                        Idle();
                }
                else
                    Move();

                break;
            case State.Attacking:
                if (target == null)
                {

                    Idle();
                }
                else
                {
                    if (Vector3.Distance(transform.position, target.position) > agent.stoppingDistance)
                    {
                        currentState = State.Moving;
                    }
                    else
                    {
                        Fire();
                    }
                }
                break;
        }
    }


    public void UpdateText(string str)
    {
        verbalText.text = "";
        verbalText.DOText(str, 1);
        verbalText.DOText("", 0).SetDelay(3);
    }
    private void Fire()
    {
        if (isRepeating) return;
        print("shooting");
        animator.SetBool("moving", false);
        animator.SetBool("attacking", true);
        // Aim at target
        transform.LookAt(target);

        // Start Firing
        shootingParticle.Play();
        isRepeating = true;
    }

    private void Idle()
    {
        currentState = State.None;
        requiredAction = State.None;
        animator.SetBool("moving", false);
        animator.SetBool("attacking", false);
        agent.stoppingDistance = 5;
        shootingParticle.Stop();
        isRepeating = false;
    }

    #region All Actions

    public void Walk()
    {
        requiredAction = State.Moving;
        currentState = State.Moving;
    }

    public void Pick_up()
    {
        requiredAction = State.pickup;
        currentState = State.Moving;
        agent.stoppingDistance = 0.5f;
    }
    public void Attack()
    {
        requiredAction = State.Attacking;
        currentState = State.Moving;
    }

    #endregion
}

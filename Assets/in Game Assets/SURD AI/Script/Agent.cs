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
        shootingParticle = machineGun;
    }


    void FixedUpdate()
    {
        if (!AI) return;
        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime * 3);
        animator.SetFloat("MoveZ", currentSpeed);

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
                        pickingUp();
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

            case State.None:
                if(autoFire)
                {
                    GetAllEnemies();
                    if (target)
                    {
                        Fire();
                    }

                }
                break;
        }
    }

    public void Apply(string functionName, Transform target = null)
    {
        SetTarget(target);
        Invoke(functionName, 0);

       // InputManager.instance.AgentRespondText(functionName + " =>> " + target.name);
    }

    private void Move()
    {
        agent.SetDestination(target.position);
        moveSpeed = 1;
    }

    private void SetTarget(Transform t)
    {
        target = t;

        // Turn the target Object into red color
       // target.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void SetAI(bool ai)
    {
        AI = ai;
        controller.AI = AI;
    }

    private async Task pickingUp()
    {
        animator.SetTrigger("pickup");
        await Task.Delay(4);
        target.SetParent(rightHand);
        target.localPosition = Vector3.zero;
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

    public void UpdateText(string str) // not in use at the moment
    {
        verbalText.text = "";
        verbalText.DOText(str, 1);
        verbalText.DOText("", 0).SetDelay(3);
    }
    private void Fire()
    {
        transform.LookAt(target);
        if (isRepeating) return;

        moveSpeed = 0;
        animator.SetBool("Aim", true);
        // Aim at target

        // Start Firing
        shootingParticle.Play();
        isRepeating = true;
    }

    public void Auto_Fire()
    {
        autoFire = true;
    }

    private void Idle()
    {
        currentState = State.None;
        requiredAction = State.None;
        moveSpeed = 0;
        animator.SetBool("Aim", false);
        agent.stoppingDistance = 5;
        shootingParticle.Stop();
        isRepeating = false;
    }

    #region All Actions

    IEnumerator Walk()
    {

        while(Vector3.Distance(transform.position, transform.forward) > 2)
        {
            agent.SetDestination(target.position);
            yield return null;
        }
        // reached
        // stop animation

        // check for futher actions from the list
    }

    //public void Walk()
    //{
    //    requiredAction = State.Moving;
    //    currentState = State.Moving;
    //}

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
        transform.LookAt(target);
        agent.stoppingDistance = attackDistance;
        //shootingParticle = machineGun;
    }

    public async void Rocket_Launcher_Attack()
    {
        autoFire = false;
        StartCoroutine(playerMovement.ChangeWeapon(1.9f / 2f));
        transform.LookAt(target);
        await Task.Delay(2000);
        shootingParticle = bazooka;
        shootingParticle.Play();
        agent.stoppingDistance = 50;
        requiredAction = State.Attacking;
        currentState = State.Moving;
    }

    public void Switch_Weapon()
    {
        StartCoroutine(playerMovement.ChangeWeapon(1.9f / 2f));
    }

    #endregion
}

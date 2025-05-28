using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using LMNT;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityLibrary;

public class Agent : MonoBehaviour
{
    public string characterName, background;
    public ParticleSystem shootingParticle;
    public ParticleSystem machineGun;
    public ParticleSystem bazooka;
    public GameObject explosive;
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
    public bool isVerbalResponse;
    private LMNTSpeech speech;
    private Processor currentAction;
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
        speech = GetComponent<LMNTSpeech>();
    }

    public void PlayAction(string functionName) // testing through Button
    {
        StartCoroutine(functionName);
    }

    public void CallFunction(Processor action)
    {
        currentAction = action;
        StopAllCoroutines();
        shootingParticle.Stop();
        SetTarget(action.target);
        StartCoroutine(action.actionKey);
        
       // print("Function Called: " +  action.actionKey);
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackDistance + 5);
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

    public void Speak(string str) // LMNT Voice to Speech
    {
        if(isVerbalResponse)
        {
            speech.dialogue = str;
            StartCoroutine(speech.Talk());
        }
    }
    private IEnumerator moving()
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
        print("reached");
        animator.SetFloat("MoveZ", 0);
    }

    private bool PathClear()
    {
        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;

        // Draw the ray in the Scene view (green if hit, red if not)
        if (Physics.Raycast(transform.position + Vector3.up, direction.normalized, out RaycastHit hit, distance))
        {
            if (hit.transform == target)
            {
                Debug.DrawRay(transform.position + Vector3.up, direction.normalized * hit.distance, Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position + Vector3.up, direction.normalized * hit.distance, Color.red);
                return false;
            }
        }
        else
        {
            Debug.DrawRay(transform.position + Vector3.up, direction.normalized * hit.distance, Color.green);
            return true;
        }

    }


    #region All Actions

    private IEnumerator Move()
    {
        agent.stoppingDistance = 1;

        // Wait until Move finishes
        yield return StartCoroutine(moving());
        agent.SetDestination(transform.position);
    }

    IEnumerator Follow()
    {
        agent.stoppingDistance = 2;
        while (true)
        {
            // Wait until Move finishes
            yield return StartCoroutine(moving());
            agent.SetDestination(transform.position);
        }
        agent.SetDestination(transform.position);
    }
    IEnumerator Attack()
    {
        agent.stoppingDistance = attackDistance;
        while (target.CompareTag("Enemy") || target.CompareTag("Player"))
        {
            // Wait until Move finishes
            yield return StartCoroutine(moving());

            // Then fire
            if (PathClear())
            {
                agent.stoppingDistance = attackDistance;
                yield return StartCoroutine(Fire());
            }
            else
                agent.stoppingDistance--;
        }
        shootingParticle.Stop();
        animator.SetBool("Aim", false);
        agent.SetDestination(transform.position);

        IO_Manager.instance.ShowSubtitle(currentAction.finalResponse);
        Speak(currentAction.finalResponse); // LMNT Voice to Speech
    }
    IEnumerator AttackAll()
    {
        autoFire = true;
        agent.stoppingDistance = attackDistance;

        GetAllEnemies();

        while (target && target.CompareTag("Enemy"))
        {
            // Wait until Move finishes
            yield return StartCoroutine(moving());

            // Then fire
            if (PathClear())
            {
                agent.stoppingDistance = attackDistance;
                yield return StartCoroutine(Fire());
            }
            else
                agent.stoppingDistance--;

            if (!target.CompareTag("Enemy"))
                GetAllEnemies();
        }
        autoFire = false;
        shootingParticle.Stop();
        agent.SetDestination(transform.position);

        IO_Manager.instance.ShowSubtitle(currentAction.finalResponse);
        Speak(currentAction.finalResponse); // LMNT Voice to Speech
    }
    IEnumerator Rocket_Launcher_Attack()
    {
        transform.DOLookAt(target.position, .5f);
        yield return StartCoroutine(Switch_Weapon());
        shootingParticle = bazooka;
        shootingParticle.Play();
        bazooka.GetComponent<AudioSource>().Play();
    }
    IEnumerator Switch_Weapon()
    {
        yield return StartCoroutine(playerMovement.ChangeWeapon(1.9f / 2f));
    }
    IEnumerator Pick_up()
    {
        yield return StartCoroutine(moving());
        animator.SetTrigger("pickup");
        yield return new WaitForSeconds(4);
        target.SetParent(rightHand);
        target.localPosition = Vector3.zero;
    }

    IEnumerator Plant()
    {
        agent.stoppingDistance = 1;
       // Transform initPos = transform;
        yield return StartCoroutine(moving());
        animator.SetTrigger("pickup");
        yield return new WaitForSeconds(3);
        Instantiate(explosive, target.position, Quaternion.identity);
        target = GameObject.FindWithTag("Player").transform;
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(moving());

        IO_Manager.instance.ShowSubtitle(currentAction.finalResponse);
        Speak(currentAction.finalResponse); // LMNT Voice to Speech
    }

    IEnumerator Lock()
    {
        transform.DOLookAt(target.position, 2).OnComplete(() =>
        {
            string response = currentAction.finalResponse + ". Waiting for your signal to fire.";
            IO_Manager.instance.ShowSubtitle(response);
            Speak(response); // LMNT Voice to Speech
            target.GetChild(target.childCount - 1).gameObject.SetActive(false); 
            Objective.Instance.value++;
        });

        animator.SetBool("Aim", true);

        while (!Input.GetMouseButtonDown(0))
        {
            transform.LookAt(target);
            yield return null; // Wait for the next frame
        }
        shootingParticle.Play();

    }
    private IEnumerator Fire()
    {
        animator.SetBool("Aim", true);
        agent.SetDestination(transform.position);
        transform.LookAt(target);
        shootingParticle.Play();
        shootingParticle.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public ParticleSystem shootingParticle;
    public Transform target;


    public void Apply(string functionName, Transform target = null)
    {
        SetTarget(target);
        Invoke(functionName, 0);

        InputManager.instance.AgentRespondText(functionName + " =>> " + target.name);
    }

    public void Move()
    {
        print("moving");
        GetComponent<NavMeshAgent>().SetDestination(target.position);
        
    }

    private void SetTarget(Transform t)
    {
        target = t;

        // Turn the target Object into red color
        target.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void Shoot()
    {
        print("shooting");

        // Aim at target
        transform.LookAt(target);

        // Start Firing
        shootingParticle.Play();
    }
}

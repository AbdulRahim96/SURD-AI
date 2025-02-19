using UnityEngine;

public class AIController : MonoBehaviour
{
    private PlayerMovement controller;
    public bool AI;
    private void Awake()
    {
        controller = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        AI = controller.AI;   
    }

    public void PlayAnim(string clipName)
    {
        controller.animator.Play(clipName);
    }
}

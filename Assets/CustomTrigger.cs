using UnityEngine;
using UnityEngine.Events;

public class CustomTrigger : MonoBehaviour
{
    public UnityEvent onTriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            onTriggerEnter.Invoke();
    }
}

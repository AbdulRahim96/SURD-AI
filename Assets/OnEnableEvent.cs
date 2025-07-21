using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableEvent : MonoBehaviour
{
    public UnityEvent onEnable, onDisable;
    public int delay = 3;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        onEnable?.Invoke();
    }
    private void OnDisable()
    {
        onDisable?.Invoke();
    }
}

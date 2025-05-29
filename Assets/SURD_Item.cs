using UnityEngine;

public class SURD_Item : MonoBehaviour
{
    public string objectName, objectDescription;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        Interactables.instance.AddItem(objectName, objectDescription, transform);
    }

    private void OnDestroy()
    {
        Interactables.instance.RemoveItem(transform);
    }
}

using UnityEngine;

public class Interactable_Modifier : MonoBehaviour
{
    public Interactables.Object interactableObject;
    public int index;
    
    public void Set()
    {
        Interactables.instance._objects[index] = interactableObject;
    }
}

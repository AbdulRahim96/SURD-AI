using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Processor : MonoBehaviour
{
    public string actionKey;
    public Transform target;
    // Start is called before the first frame update
    

    public Processor(string[] words, ActionHandler actionHandler, Interactables interactables)
    {
        foreach (var word in words)
        {
            if(actionHandler.Check(word) == true)
            {
                actionKey = word;
                break;
            }
        }
        foreach (var word in words)
        {
            int index = interactables.Check(word);
            if (index != -1)
            {
                target = interactables.GetTarget(index);
            }
        }
    }
}

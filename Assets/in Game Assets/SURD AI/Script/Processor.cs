using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Processor : MonoBehaviour
{
    public string verbalResponse, finalResponse;
    public string actionKey;
    public List<string> keys;
    public Transform target;
    // Start is called before the first frame update
    

    public Processor(SURD_AI.ResponseData response, Interactables interactables)
    {
        /* foreach (var word in words)
         {
             if(actionHandler.Check(word) == true)
             {
                 actionKey = word;
                 break;
             }
         }*/
        verbalResponse = response.verbal_Response;
        finalResponse = response.final_Response;

        if (response.actions.Length == 0) return;
        actionKey = response.actions[0];

        if (response.entities.Length == 0) return;
        int index = interactables.Check(response.entities[0]);
        if (index != -1)
        {
            target = interactables.GetTarget(index);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    private Agent agent;
    public List<Actions> actions;

    private void Awake()
    {
        agent = GetComponent<Agent>();
    }
    public bool Check(string word)
    {
        foreach (var item in actions)
        {
            if (word == item.actionName)
                return true;
        }
        return false;
    }

    public void DoAction(Processor processor)
    {
        agent.CallFunction(processor);
       // agent.Speak(processor.verbalResponse);

        /*foreach (var item in actions)
        {
            if(processor.actionKey == item.actionName)
            {
                agent.Apply(processor.actionKey, processor.target);
            }
        }*/
    }

    public void AddAction(string actionName, string description)
    {
        Actions newAction = new Actions
        {
            actionName = actionName,
            description = description
        };
        actions.Add(newAction);
    }

    public void RemoveActionByName(string actionName)
    {
        Actions actionToRemove = actions.Find(a => a.actionName == actionName);
        if (actionToRemove != null)
        {
            actions.Remove(actionToRemove);
        }
    }

    public void RemoveActionByIndex(int index) => actions.RemoveAt(index);

    [System.Serializable]
    public class Actions
    {
        public string actionName;
        public string description;
    }
}

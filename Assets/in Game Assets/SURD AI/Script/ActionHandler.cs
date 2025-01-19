using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    private Agent agent;
    public Actions[] actions;

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
        foreach (var item in actions)
        {
            if(processor.actionKey == item.actionName)
            {
                agent.Apply(processor.actionKey, processor.target);
            }
        }
    }

    [System.Serializable]
    public class Actions
    {
        public string actionName;
        public string description;
    }
}

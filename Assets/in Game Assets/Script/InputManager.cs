using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public InputField inputField;
    public Transform messageList;
    public Text textPrefab;


    public Agent currentActiveAgent;
    [TextArea(1, 5)]
    public string sentence, inputStructure;
    public string[] words;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartProcess();
    }

    public void UpdateSentence(string str)
    {
        sentence = str;
    }

    private void SetStructure(ActionHandler actionList, Interactables objectsList)
    {
        string actions = "Available actions: ";
        foreach (var action in actionList.actions)
        {
            actions += action.actionName + " (" + action.description + "), ";
        }

        string objects = "Objects list: ";
        foreach (var obj in objectsList._objects)
        {
            objects += obj.objectName + " (" + obj.description + "), ";
        }

        string personality = "Personality / Backstory: " + "A disciplined and brave soldier" + ", ";
        string userInput = "Natural language input: " + sentence;

        string format = actions + objects + personality + userInput;
        inputStructure = format;

    }

    void SplitIntoWords()
    {
        // Split the sentence based on spaces
        words = sentence.Split(' ');
        // words = sentence.Split(new char[] { ' ', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
    }
    public void StartProcess()
    {
        SetStructure(currentActiveAgent.GetComponent<ActionHandler>(), Interactables.instance);
        SURD_AI.Instance.SendToSURDModel(inputStructure);
        /*
        SplitIntoWords();

        inputField.text = "";

        Processor p = new Processor(words, currentActiveAgent.GetComponent<ActionHandler>(), Interactables.instance);
        currentActiveAgent.GetComponent<ActionHandler>().DoAction(p);*/
    }

    public void AgentRespondText(string str)
    {
        Text text = Instantiate(textPrefab, messageList);
        text.text = str;
    }
}

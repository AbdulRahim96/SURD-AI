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
    public string sentence;
    public string[] words;
    private void Awake()
    {
        instance = this;
    }

    public void UpdateSentence(string str)
    {
        sentence = str;
    }

    void SplitIntoWords()
    {
        // Split the sentence based on spaces
        words = sentence.Split(' ');
        // words = sentence.Split(new char[] { ' ', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
    }
    public void StartProcess()
    {

        SplitIntoWords();

        inputField.text = "";

        Processor p = new Processor(words, currentActiveAgent.GetComponent<ActionHandler>(), Interactables.instance);
        currentActiveAgent.GetComponent<ActionHandler>().DoAction(p);
    }

    public void AgentRespondText(string str)
    {
        Text text = Instantiate(textPrefab, messageList);
        text.text = str;
    }
}

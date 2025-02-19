using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class IO_Manager : MonoBehaviour
{
    public static IO_Manager instance;
    public InputField inputField;
    public Text subtitleText;
    public bool isTesting;
    public string testinput;
    public VoskSpeechToText voiceInput;

    public Agent currentActiveAgent;
    [TextArea(1, 5)]
    public string sentence, inputStructure;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        voiceInput.OnTranscriptionResult += onVoiceComplete;
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

        string personality = "Personality / Backstory: Name is " + currentActiveAgent.characterName + ", " + currentActiveAgent.background + ", ";
        string userInput = "Natural language input: " + sentence;

        string format = actions + objects + personality + userInput;
        inputStructure = format;

    }

    public async void StartProcess()
    {
        print("Start Processing..");
        SetStructure(currentActiveAgent.GetComponent<ActionHandler>(), Interactables.instance);
        //SURD_AI.Instance.SendToSURDModel(inputStructure);
        SURD_AI.ResponseData responseData;
        if (isTesting)
        {
            responseData = await SURD_AI.Instance.CallAsyncAIModelTest(testinput);
        }
        else
        {
            responseData = await SURD_AI.Instance.CallAsyncAIModel(inputStructure);
        }
        
        Processor p = new Processor(responseData, Interactables.instance);
        ShowSubtitle(p.verbalResponse);
        currentActiveAgent.GetComponent<ActionHandler>().DoAction(p);
    }

    public void ShowSubtitle(string str)
    {
        string subtitle = currentActiveAgent.characterName + ": " + str;
        //responseText = textLayout.GetComponentInChildren<Text>();

        // textLayout.transform.DOScaleX(1, 1).SetEase(Ease.OutBack).OnStart(() => textLayout.SetActive(true));
        // textLayout.transform.DOScaleX(0, 1).SetDelay(5).OnComplete(()=> textLayout.SetActive(false));
        subtitleText.text = "";
        subtitleText.DOText(subtitle, 0.1f);
        subtitleText.DOText("", 0).SetDelay(5);
    }

    public void UserInput(string str)
    {
        string subtitle = "You: " + str;
        //responseText = textLayout.GetComponentInChildren<Text>();

        // textLayout.transform.DOScaleX(1, 1).SetEase(Ease.OutBack).OnStart(() => textLayout.SetActive(true));
        // textLayout.transform.DOScaleX(0, 1).SetDelay(5).OnComplete(()=> textLayout.SetActive(false));
        subtitleText.text = subtitle;
        UpdateSentence(str);
        StartProcess();
    }


    private void onVoiceComplete(string dialogues)
    {
        var result = new RecognitionResult(dialogues);
        print("Best Result: " + result.Phrases[0].Text);
        string str = result.Phrases[0].Text;
        UserInput(str);
    }
}
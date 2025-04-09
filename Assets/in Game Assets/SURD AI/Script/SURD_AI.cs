using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static ParsingTest;
using System.Threading.Tasks;
using UnityEngine.Windows;
using UnityEditor.PackageManager.Requests;
using static callReq;

public class SURD_AI : MonoBehaviour
{
    public static SURD_AI Instance;
    [SerializeField] private string apiUrl = "https://surdaimodelserver.vercel.app/ai-model";

    public AIModelInput modelInput;
    public ResultData _response;


    private void Awake()
    {
        Instance = this;
      //  CallAsyncAIModel("");
    }

    public void SendToSURDModel(string input) // not in use
    {
        modelInput.input = input;
        StartCoroutine(CallAIModel(modelInput));
    }
    public IEnumerator CallAIModel(AIModelInput payload)
    {
        string jsonPayload = JsonUtility.ToJson(payload);

        // Create UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        print("sending to Model");
        // Send the request
        yield return request.SendWebRequest();

        // Handle the response
        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            AIModelResponse response = JsonUtility.FromJson<AIModelResponse>(jsonResponse);
           // Debug.Log("AI Response: " + response.response);

            print(jsonResponse);

            try
            {
                // Parse the JSON string
                _response = JsonConvert.DeserializeObject<ResultData>(jsonResponse);
                /*
                // Access values
                Debug.Log("Action: " + _response.AIResponse.actions[0]);
                Debug.Log("Entity: " + _response.AIResponse.entities[0]);
                Debug.Log("Response: " + _response.AIResponse.verbalResponse);*/
            }
            catch (JsonException ex)
            {
                Debug.LogError("JSON Parsing Error: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    public async Task<ResponseData> CallAsyncAIModel(string input)
    {
        modelInput.input = input;
        string jsonPayload = JsonUtility.ToJson(modelInput);

        // Create UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        print("sending to Model");
        await request.SendWebRequest();

        // clearing previous data
       // _response.Clear();

        // Handle the response
        if (request.result == UnityWebRequest.Result.Success)
        {
            _response.recievedOutput = request.downloadHandler.text;
            print("Recieved => " + _response.recievedOutput);
            // Parse the JSON string
            _response = JsonConvert.DeserializeObject<ResultData>(_response.recievedOutput);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            _response.AI_Response.verbal_Response = "I don't understand what you are saying";
        }
        return _response.AI_Response;
    }

    public async Task<ResponseData> CallAsyncAIModelTest(string input, int val = 3)
    {

        await Task.Delay(val * 1000);
        string jsonResponse = input;
        print(jsonResponse);
        try
        {
            // Parse the JSON string
            _response = JsonConvert.DeserializeObject<ResultData>(jsonResponse);
            _response.recievedOutput = jsonResponse;
            return _response.AI_Response;
        }
        catch (JsonException ex)
        {
            Debug.LogError("JSON Parsing Error: " + ex.Message);
        }
        return null;
    }

    [System.Serializable]
    public class AIModelInput
    {
        [TextArea(3, 10)]
        public string system_prompt;
        [TextArea(1, 5)]
        public string input;
        public string model;
        public float temperature;
        public float top_p;
    }

    [System.Serializable]
    // Root class matching the JSON structure
    public class ResultData
    {
        public ResponseData AI_Response;
        [HideInInspector] public string recievedOutput;

        public void Clear()
        {
            AI_Response = null;
        }
    }

    [System.Serializable]
    // Nested class for the 'response' object
    public class ResponseData
    {
        public string[] actions;
        public string[] entities;
        public string verbal_Response;
        public string final_Response;
    }

}

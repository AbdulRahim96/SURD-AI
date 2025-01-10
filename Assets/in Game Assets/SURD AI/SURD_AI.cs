using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SURD_AI : MonoBehaviour
{
    public static SURD_AI Instance;
    [SerializeField] private string apiUrl = "https://c285-111-88-39-203.ngrok-free.app/ai-model";

    public AIModelInput modelInput;

    private void Awake()
    {
        Instance = this;
    }

    public void SendToSURDModel(string input)
    {
        modelInput.input = input;
       // StartCoroutine(CallAIModel(modelInput));
    }
    public IEnumerator CallAIModel(AIModelInput payload)
    {
        string jsonPayload = JsonUtility.ToJson(payload);

        // Create UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request
        yield return request.SendWebRequest();

        // Handle the response
        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            AIModelResponse response = JsonUtility.FromJson<AIModelResponse>(jsonResponse);
            Debug.Log("AI Response: " + response.response);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    [System.Serializable]
    public class AIModelInput
    {
        [TextArea(1, 5)]
        public string input;
        public string model;
        public float temperature;
        public float top_p;
    }

    [System.Serializable]
    public class AIModelResponse
    {
        public string response;
    }

}

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class callReq : MonoBehaviour
{
    public string input, model;
    public float temp, top;
    public AIModelInput modelInput;

    private void Start()
    {
        StartCoroutine(CallAIModel(modelInput);
    }

    public IEnumerator CallAIModel(AIModelInput payload)
    {
        string url = "https://c285-111-88-39-203.ngrok-free.app/ai-model";


        string jsonPayload = JsonUtility.ToJson(payload);

        // Create UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(url, "POST");
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

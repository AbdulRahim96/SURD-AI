using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SURD_AI : MonoBehaviour
{
    [SerializeField] private string apiUrl = "https://c285-111-88-39-203.ngrok-free.app/ai-model";

    private void Start()
    {
        CallAIModel("Hello there");
    }

    public void CallAIModel(string input)
    {
        // Start the coroutine to send the request
        StartCoroutine(SendRequest(input));
    }

    private IEnumerator SendRequest(string input)
    {
        // Create the JSON payload
        //string jsonPayload = JsonUtility.ToJson(new AIRequest { input = input });

        // Create a UnityWebRequest
        using (UnityWebRequest webRequest = new UnityWebRequest(apiUrl, "POST"))
        {
            // Attach JSON payload to the request
            //byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            //webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            print("sending...");
            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Handle the response
                Debug.Log("Response: " + webRequest.downloadHandler.text);
            }
            else
            {
                // Handle the error
                Debug.LogError("Error: " + webRequest.error);
            }
        }
    }

    [System.Serializable]
    public class AIRequest
    {
        public string input;
    }

}

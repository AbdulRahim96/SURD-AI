using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class ParsingTest : MonoBehaviour
{
    [TextArea(7, 10)] public string testString;
    public RootData outputData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Parse();
    }

    // Update is called once per frame
    void Parse()
    {
        try
        {
            // Parse the JSON string
            outputData = JsonConvert.DeserializeObject<RootData>(testString);
            outputData.outputRecieved = "recieved";
            // Access values
          /*  Debug.Log("Action: " + outputData.AI_Response.actions[0]);
            Debug.Log("Entity: " + outputData.AI_Response.entities[0]);
            Debug.Log("Response: " + outputData.AI_Response.verbal_response);*/
            print(testString);
        }
        catch (JsonException ex)
        {
            Debug.LogError("JSON Parsing Error: " + ex.Message);
        }
    }
    [System.Serializable]
    // Root class matching the JSON structure
    public class RootData
    {
        public ResponseData AI_Response;
        public string outputRecieved;
    }

    [System.Serializable]
    // Nested class for the 'response' object
    public class ResponseData
    {
        public string[] actions;
        public string[] entities;
        public string verbal_response;
    }
}

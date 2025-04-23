using UnityEngine;

[CreateAssetMenu(fileName = "SystemPromptObject", menuName = "Scriptable Objects/SystemPromptObject")]
public class SystemPrompt_Object : ScriptableObject
{
    [TextArea(3, 10)] public string roleDescription; // About what the AI does
    [TextArea(3, 10)] public string inputInstructions; // Explains input format
    [TextArea(3, 10)] public string outputInstructions; // Describes expected output
    [TextArea(3, 10)] public string outputFormatExample; // Shows sample JSON output

    public string GetFullSystemInput()
    {
        return roleDescription + "\n\n" +
               inputInstructions + "\n\n" +
               outputInstructions + "\n\n" +
               outputFormatExample;
    }
}

using UnityEngine;

public class AudioListen : MonoBehaviour
{
    public VoiceProcessor voiceProcessor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        voiceProcessor.OnRecordingStart += DisableAudio;
        voiceProcessor.OnRecordingStop += EnableAudio;
    }

    void EnableAudio()
    {
        GetComponent<AudioListener>().enabled = true;
    }

    void DisableAudio()
    {
        GetComponent <AudioListener>().enabled = false;
    }
}

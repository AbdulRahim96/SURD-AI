using UnityEngine;
using DG.Tweening;

public class VoiceIcon : MonoBehaviour
{
    public VoiceProcessor voiceProcessor;
    private DOTweenAnimation animation;

    private void Start()
    {
        animation = GetComponent<DOTweenAnimation>();
        voiceProcessor.OnRecordingStart += StartAnim;
        voiceProcessor.OnRecordingStop += StopAnim;
    }
    private void StartAnim()
    {
        animation.DORestart();
    }

    private void StopAnim()
    {
        animation.DORewind();
    }

}
